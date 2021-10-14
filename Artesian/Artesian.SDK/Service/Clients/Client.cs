// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
#if !NET452
using Ark.Tools.Auth0;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using JWT.Builder;
#endif
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ark.Tools.Http;

namespace Artesian.SDK.Service
{
    internal sealed class Client : IDisposable
    {
        private readonly IAuthenticationApiClient _auth0;
        private readonly ClientCredentialsTokenRequest _credentials;
        private readonly MediaTypeFormatterCollection _formatters;
        private readonly IFlurlClient _client;

        private readonly JsonMediaTypeFormatter _jsonFormatter;
        private readonly MessagePackMediaTypeFormatter _msgPackFormatter;
        private readonly LZ4MessagePackMediaTypeFormatter _lz4msgPackFormatter;


        private readonly string _url;
        private readonly AsyncPolicy _resilienceStrategy;
        private readonly string _apiKey;

        /// <summary>
        /// Client constructor Auth credentials / ApiKey can be passed through config
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="Url">String</param>
        /// /// <param name="policy">String</param>
        public Client(IArtesianServiceConfig config, string Url, ArtesianPolicyConfig policy)
        {
            _url = config.BaseAddress.ToString().AppendPathSegment(Url);
            _apiKey = config.ApiKey;

            var cfg = new JsonSerializerSettings();
            cfg = cfg.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            cfg = cfg.ConfigureForDictionary();
            cfg = cfg.ConfigureForNodaTimeRanges();
            cfg.Formatting = Formatting.Indented;
            cfg.ContractResolver = new DefaultContractResolver();
            cfg.Converters.Add(new StringEnumConverter());
            cfg.TypeNameHandling = TypeNameHandling.None;
            cfg.ObjectCreationHandling = ObjectCreationHandling.Replace;

            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = cfg
            };
            _jsonFormatter = jsonFormatter;
            _jsonFormatter.SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/problem+json"));

            _msgPackFormatter = new MessagePackMediaTypeFormatter(CustomCompositeResolver.Instance);
            _lz4msgPackFormatter = new LZ4MessagePackMediaTypeFormatter(CustomCompositeResolver.Instance);
            //Order of formatters important for correct weight in accept header
            var formatters = new MediaTypeFormatterCollection();
            formatters.Clear();
            formatters.Add(_lz4msgPackFormatter);
            formatters.Add(_msgPackFormatter);
            formatters.Add(_jsonFormatter);
            _formatters = formatters;

            _resilienceStrategy = policy.GetResillianceStrategy();

            if (config.ApiKey == null)
            {
				_auth0 = new AuthenticationApiClientCachingDecorator(new AuthenticationApiClient($"{config.Domain}"));

                _credentials = new ClientCredentialsTokenRequest()
                {
                    Audience = config.Audience,
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                };
            }

            _client = new FlurlClient(_url);
            _client.WithTimeout(TimeSpan.FromMinutes(ArtesianConstants.ServiceRequestTimeOutMinutes));
        }

       
        public async Task<TResult> Exec<TResult, TBody>(HttpMethod method, string resource, TBody body = default, CancellationToken ctk = default)
        {
            try
            {
                var req = _client.Request(resource).WithAcceptHeader(_formatters).AllowAnyHttpStatus();

                req = req.WithHeader("X-Artesian-Agent", ArtesianConstants.SDKVersionHeaderValue);

                if (_apiKey != null)
                    req = req.WithHeader("X-Api-Key", _apiKey);
                else
                {
                    var res = await _auth0.GetTokenAsync(_credentials, ctk);
                    req = req.WithOAuthBearerToken(res.AccessToken);
                }

                ObjectContent content = null;

                try
                {
                    if (body != null)
                        content = new ObjectContent(typeof(TBody), body, _lz4msgPackFormatter);

                    using (var res =  await _resilienceStrategy.ExecuteAsync ( () =>  req.SendAsync(method, content: content, completionOption: HttpCompletionOption.ResponseContentRead, cancellationToken: ctk)))
                    {
                        if (res.ResponseMessage.StatusCode == HttpStatusCode.NoContent || res.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
                            return default;

                        if (!res.ResponseMessage.IsSuccessStatusCode)
                        {
                            ArtesianSdkProblemDetail problemDetail = null;
                            string responseText = string.Empty;

                            if (res.ResponseMessage.Content.Headers.ContentType.MediaType == "application/problem+json")
                            {
                                problemDetail = await res.ResponseMessage.Content.ReadAsAsync<ArtesianSdkProblemDetail>(_formatters, ctk);
                            }
                            else
                            {
                                if (res.ResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    responseText = _tryDecodeText(await res.ResponseMessage.Content.ReadAsAsync<object>(_formatters, ctk));
                                }
                                else
                                {
                                    responseText = await res.ResponseMessage.Content.ReadAsStringAsync();
                                }
                            }

                            var detailMessage = problemDetail?.Detail ?? problemDetail?.Title ?? problemDetail?.Type ?? "Content:" + Environment.NewLine + responseText;
                            var exceptionMessage = $"Failed handling REST call to WebInterface {method} {_url + resource}. Returned status: {res.StatusCode}. {detailMessage}";

                            switch (res.ResponseMessage.StatusCode)
                            {
                                case HttpStatusCode.BadRequest:
                                    throw new ArtesianSdkValidationException(exceptionMessage, problemDetail);
                                case HttpStatusCode.Conflict:
                                case HttpStatusCode.PreconditionFailed:
                                    throw new ArtesianSdkOptimisticConcurrencyException(exceptionMessage, problemDetail);
                                case HttpStatusCode.Forbidden:
                                    throw new ArtesianSdkForbiddenException(exceptionMessage, problemDetail);
                                default:
                                    throw new ArtesianSdkRemoteException(exceptionMessage, problemDetail);
                            }
                        }

                        return await res.ResponseMessage.Content.ReadAsAsync<TResult>(_formatters, ctk);
                    }
                }
                finally
                {
                    content?.Dispose();
                }
            }
            catch (ArtesianSdkRemoteException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ArtesianSdkClientException($"Failed handling REST call to WebInterface: {method} " + _url + resource, e);
            }
        }

        private string _tryDecodeText(object responseDeserialized)
        {
            switch (responseDeserialized)
            {
                case Dictionary<object, object> dc:
                    {
                        if (dc.Count > 0)
                        {
                            if (dc.ContainsKey("ErrorMessage"))
                            {
                                return dc["ErrorMessage"].ToString();
                            }
                        }
                        break;
                    }
                case String st:
                    {
                        return st;
                    }
                case Int32 i:
                    {
                        return i.ToString();
                    }
                default:
                    return "Not parsed error message";
            }

            return null;
        }

        public async Task Exec(HttpMethod method, string resource, CancellationToken ctk = default)
            => await Exec<object, object>(method, resource, null, ctk);

        public Task<TResult> Exec<TResult>(HttpMethod method, string resource, CancellationToken ctk = default)
            => Exec<TResult, object>(method, resource, null, ctk);

        public async Task Exec<TBody>(HttpMethod method, string resource, TBody body, CancellationToken ctk = default)
            => await Exec<object, TBody>(method, resource, body, ctk);

        public void Dispose()
        {
            _client.Dispose();
        }

    }
    /// <summary>
    /// Flurl Extension
    /// </summary>
    public static class FlurlExt
    {
        /// <summary>
        /// Flurl request extension to return Accept headers
        /// </summary>
        /// <param name="request">IFlurlRequest</param>
        /// <param name="formatters">MediaTypeFormatterCollection</param>
        /// <returns></returns>
        public static IFlurlRequest WithAcceptHeader(this IFlurlRequest request, MediaTypeFormatterCollection formatters)
        {
            var cnt = formatters.Count;
            var step = 1.0 / (cnt + 1);
            var sb = new StringBuilder();
            var headers = formatters.Select((x, i) => new MediaTypeWithQualityHeaderValue(x.SupportedMediaTypes.First().MediaType, 1 - (step * i)));

            return request.WithHeader("Accept", string.Join(",", headers));
        }
    }
}
