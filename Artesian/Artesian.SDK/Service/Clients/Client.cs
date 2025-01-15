// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Flurl;
using Flurl.Http;

using Microsoft.Identity.Client;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using NodaTime;
using NodaTime.Serialization.JsonNet;

using Polly;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Accepted to 'leak' FlurlClient instance")]
    internal sealed class Client
    {
        private readonly MediaTypeFormatterCollection _formatters;
        private readonly FlurlClient _client;

        private readonly JsonMediaTypeFormatter _jsonFormatter;
        private readonly MessagePackMediaTypeFormatter _msgPackFormatter;
        private readonly LZ4MessagePackMediaTypeFormatter _lz4msgPackFormatter;


        private readonly string _url;
        private readonly AsyncPolicy _resilienceStrategy;
        private readonly string _apiKey;
        private readonly IArtesianServiceConfig _config;

        private readonly IConfidentialClientApplication _confidentialClientApplication;

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
            _config = config;

            var cfg = new JsonSerializerSettings();
            cfg = cfg.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            cfg = cfg.ConfigureForDictionary();
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
                var domain = new Uri(config.Domain);

                var tenantId = domain.Segments
                    .Select(s => s.Trim('/'))
                    .FirstOrDefault(w => !string.IsNullOrWhiteSpace(w));

                _confidentialClientApplication = ConfidentialClientApplicationBuilder
                              .Create(config.ClientId)
                              .WithTenantId(tenantId)
                              .WithClientSecret(config.ClientSecret)
                              .Build();
            }

            _client = new FlurlClient(_url);
            _client.WithTimeout(TimeSpan.FromMinutes(ArtesianConstants._serviceRequestTimeOutMinutes));
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
                    var c = _confidentialClientApplication
                        .AcquireTokenForClient(new[] { _config.Audience });
                    var res = await c.ExecuteAsync(ctk).ConfigureAwait(false);
                    req = req.WithOAuthBearerToken(res.AccessToken);
                }

                ObjectContent content = null;

                try
                {
                    if (body != null)
                        content = new ObjectContent(typeof(TBody), body, _lz4msgPackFormatter);

                    using (var res =  await _resilienceStrategy.ExecuteAsync ( () =>  req.SendAsync(method, content: content, completionOption: HttpCompletionOption.ResponseContentRead, cancellationToken: ctk)).ConfigureAwait(false))
                    {
                        if (res.ResponseMessage.StatusCode == HttpStatusCode.NoContent || res.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
                            return default;

                        if (!res.ResponseMessage.IsSuccessStatusCode)
                        {
                            ArtesianSdkProblemDetail problemDetail = null;
                            string responseText = string.Empty;

                            if (res.ResponseMessage.Content.Headers.ContentType.MediaType == "application/problem+json")
                            {
                                problemDetail = await res.ResponseMessage.Content.ReadAsAsync<ArtesianSdkProblemDetail>(_formatters, ctk).ConfigureAwait(false);
                            }
                            else
                            {
                                if (res.ResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                                {
                                    var obj = await res.ResponseMessage.Content.ReadAsAsync<object>(_formatters, ctk).ConfigureAwait(false);
                                    responseText = _tryDecodeText(obj);
                                }
                                else
                                {
                                    responseText = await res.ResponseMessage.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
                                        ctk
#endif
                                    ).ConfigureAwait(false);
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

                        return await res.ResponseMessage.Content.ReadAsAsync<TResult>(_formatters, ctk).ConfigureAwait(false);
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

        private static string _tryDecodeText(object responseDeserialized)
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
                        return i.ToString(CultureInfo.InvariantCulture);
                    }
                default:
                    return "Not parsed error message";
            }

            return null;
        }

        public async Task Exec(HttpMethod method, string resource, CancellationToken ctk = default)
            => await Exec<object, object>(method, resource, null, ctk).ConfigureAwait(false);

        public Task<TResult> Exec<TResult>(HttpMethod method, string resource, CancellationToken ctk = default)
            => Exec<TResult, object>(method, resource, null, ctk);

        public async Task Exec<TBody>(HttpMethod method, string resource, TBody body, CancellationToken ctk = default)
            => await Exec<object, TBody>(method, resource, body, ctk).ConfigureAwait(false);
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
