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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Accepted to 'leak' FlurlClient instance")]
    internal sealed class Client
    {
        private readonly List<IContentSerializer> _serializers;
        private readonly FlurlClient _client;

        private readonly JsonContentSerializer _jsonSerializer;
        private readonly MessagePackContentSerializer _msgPackSerializer;
        private readonly LZ4MessagePackContentSerializer _lz4msgPackSerializer;

        private readonly string _url;
        private readonly AsyncPolicy? _resilienceStrategy;
        private readonly string? _apiKey;
        private readonly IArtesianServiceConfig _config;

        private readonly IConfidentialClientApplication? _confidentialClientApplication;

        /// <summary>
        /// Client constructor Auth credentials / ApiKey can be passed through config
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="Url">String</param>
        /// /// <param name="policy">String</param>
        public Client(IArtesianServiceConfig config, string Url, ArtesianPolicyConfig policy)
        {
            if (config.BaseAddress == null)
                throw new ArgumentException("BaseAddress cannot be null", nameof(config));
            
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

            _jsonSerializer = new JsonContentSerializer(cfg);
            _msgPackSerializer = new MessagePackContentSerializer(CustomCompositeResolver.Instance ?? throw new InvalidOperationException("CustomCompositeResolver.Instance is null"));
            _lz4msgPackSerializer = new LZ4MessagePackContentSerializer(CustomCompositeResolver.Instance ?? throw new InvalidOperationException("CustomCompositeResolver.Instance is null"));

            // Order is important for quality values in Accept header
            _serializers = new List<IContentSerializer>
            {
                _lz4msgPackSerializer,
                _msgPackSerializer,
                _jsonSerializer
            };

            _resilienceStrategy = policy.GetResillianceStrategy();

            if (config.ApiKey == null)
            {
                if (config.Domain == null)
                    throw new ArgumentException("Domain cannot be null when ApiKey is not provided", nameof(config));
                if (config.ClientId == null)
                    throw new ArgumentException("ClientId cannot be null when ApiKey is not provided", nameof(config));
                if (config.ClientSecret == null)
                    throw new ArgumentException("ClientSecret cannot be null when ApiKey is not provided", nameof(config));
                if (config.Audience == null)
                    throw new ArgumentException("Audience cannot be null when ApiKey is not provided", nameof(config));
                
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

       
        public async Task<TResult> Exec<TResult, TBody>(HttpMethod method, string resource, TBody? body = default, CancellationToken ctk = default)
        {
            try
            {
                var req = _client.Request(resource).WithAcceptHeader(_serializers).AllowAnyHttpStatus();

                req = req.WithHeader("X-Artesian-Agent", ArtesianConstants.SDKVersionHeaderValue);

                if (_apiKey != null)
                    req = req.WithHeader("X-Api-Key", _apiKey);
                else
                {
                    if (_confidentialClientApplication == null)
                        throw new InvalidOperationException("ConfidentialClientApplication not initialized");
                    if (_config.Audience == null)
                        throw new InvalidOperationException("Audience not configured");
                    
                    var c = _confidentialClientApplication
                        .AcquireTokenForClient(new[] { _config.Audience });
                    var res = await c.ExecuteAsync(ctk).ConfigureAwait(false);
                    req = req.WithOAuthBearerToken(res.AccessToken);
                }

                HttpContent? content = null;

                try
                {
                    if (body != null)
                    {
                        // Create a custom HttpContent that serializes directly without buffering
                        content = new SerializerStreamContent<TBody>(_lz4msgPackSerializer, body, ctk);
                        content.Headers.ContentType = new MediaTypeHeaderValue(_lz4msgPackSerializer.MediaType);
                    }

                    if (_resilienceStrategy == null)
                    {
                        throw new InvalidOperationException("Resilience strategy not initialized");
                    }

                    return await _resilienceStrategy.ExecuteAsync(async () =>
                    {
                        using (var res = await req.SendAsync(method, content: content, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: ctk).ConfigureAwait(false))
                        {
                            if (res.ResponseMessage.StatusCode == HttpStatusCode.NoContent || res.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
                                return default!;

                            if (!res.ResponseMessage.IsSuccessStatusCode)
                            {
                                ArtesianSdkProblemDetail? problemDetail = null;
                                string responseText = string.Empty;

                                var contentType = res.ResponseMessage.Content.Headers.ContentType?.MediaType;
                                
                                // Flurl.Http.Testing might set Content-Type as a response header instead of content header
                                // Try multiple variations: "Content-Type", "Content_Type", and "ContentType"
                                if (string.IsNullOrEmpty(contentType))
                                {
                                    if (res.ResponseMessage.Headers.TryGetValues("Content-Type", out var ct1Values))
                                    {
                                        contentType = ct1Values.FirstOrDefault()?.Split(';')[0].Trim();
                                    }
                                    else if (res.ResponseMessage.Headers.TryGetValues("Content_Type", out var ct2Values))
                                    {
                                        contentType = ct2Values.FirstOrDefault()?.Split(';')[0].Trim();
                                    }
                                    else if (res.ResponseMessage.Headers.TryGetValues("ContentType", out var ct3Values))
                                    {
                                        contentType = ct3Values.FirstOrDefault()?.Split(';')[0].Trim();
                                    }
                                }

                                // If content type is exactly "application/problem+json", deserialize directly as ProblemDetail without buffering
                                // The server guarantees this is a ProblemDetail response, so let any deserialization exceptions bubble up
                                if (string.Equals(contentType, "application/problem+json", StringComparison.OrdinalIgnoreCase))
                                {
                                    var stream = await res.ResponseMessage.Content.ReadAsStreamAsync(
#if NET6_0_OR_GREATER
                                        ctk
#endif
                                    ).ConfigureAwait(false);
                                    
                                    problemDetail = await _jsonSerializer.DeserializeAsync<ArtesianSdkProblemDetail>(stream, ctk).ConfigureAwait(false);
                                }
                                else
                                {
                                    // For other error responses, read as text directly (no deserialization needed)
                                    // Include first 1000 chars in exception details
                                    var fullText = await res.ResponseMessage.Content.ReadAsStringAsync(
#if NET6_0_OR_GREATER
                                        ctk
#endif
                                    ).ConfigureAwait(false);
                                    
                                    responseText = fullText.Length > 1000 ? fullText.Substring(0, 1000) : fullText;
                                }

                                string detailMessage;
                                if (problemDetail != null)
                                {
                                    // Build message from ProblemDetail fields
                                    var parts = new List<string>();
                                    if (problemDetail.Title != null && !string.IsNullOrEmpty(problemDetail.Title))
                                        parts.Add(problemDetail.Title);
                                    if (problemDetail.Detail != null && !string.IsNullOrEmpty(problemDetail.Detail))
                                        parts.Add(problemDetail.Detail);
                                    if (parts.Count == 0 && problemDetail.Type != null && !string.IsNullOrEmpty(problemDetail.Type))
                                        parts.Add(problemDetail.Type);
                                    
                                    detailMessage = parts.Count > 0 ? string.Join(": ", parts) : responseText;
                                }
                                else
                                {
                                    detailMessage = "Content:" + Environment.NewLine + responseText;
                                }
                                
                                var exceptionMessage = $"Failed handling REST call to WebInterface {method} {_url + resource}. Returned status: {res.StatusCode}. {detailMessage}";

                                // Throw appropriate exception based on status code
                                // 4xx errors should not be retried by the resilience strategy
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

                            // For successful responses, handle deserialization
                            var contentLength = res.ResponseMessage.Content.Headers.ContentLength;
                            if (contentLength.HasValue && contentLength.Value == 0)
                                return default;

                            var responseStream = await res.ResponseMessage.Content.ReadAsStreamAsync(
#if NET6_0_OR_GREATER
                                ctk
#endif
                            ).ConfigureAwait(false);

                            var responseContentType = res.ResponseMessage.Content.Headers.ContentType?.MediaType;
                            
                            // Flurl.Http.Testing might set Content-Type as a response header instead of content header
                            // Try both "Content-Type" and "ContentType" (Flurl might not convert the property name)
                            if (string.IsNullOrEmpty(responseContentType))
                            {
                                if (res.ResponseMessage.Headers.TryGetValues("Content-Type", out var ct1Values))
                                {
                                    responseContentType = ct1Values.FirstOrDefault()?.Split(';')[0].Trim();
                                }
                                else if (res.ResponseMessage.Headers.TryGetValues("Content_Type", out var ct2Values))
                                {
                                    responseContentType = ct2Values.FirstOrDefault()?.Split(';')[0].Trim();
                                }
                                else if (res.ResponseMessage.Headers.TryGetValues("ContentType", out var ct3Values))
                                {
                                    responseContentType = ct3Values.FirstOrDefault()?.Split(';')[0].Trim();
                                }
                            }
                            
                            var responseBaseMediaType = _getBaseMediaType(responseContentType);
                            var responseSerializer = _getSerializer(responseBaseMediaType);
                            
                            if (responseSerializer == null)
                            {
                                throw new ArtesianSdkClientException($"Unsupported content type: {responseContentType}. Supported types are: application/json, application/x-msgpack, application/x.msgpacklz4");
                            }

                            return await responseSerializer.DeserializeAsync<TResult>(responseStream, ctk).ConfigureAwait(false);
                        }
                    }).ConfigureAwait(false);
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
            catch (ArtesianSdkClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ArtesianSdkClientException($"Failed handling REST call to WebInterface: {method} " + _url + resource, e);
            }
        }

        private string? _getBaseMediaType(string? mediaType)
        {
            if (string.IsNullOrEmpty(mediaType))
                return null;

            // Remove parameters (e.g., "application/json; charset=utf-8" -> "application/json")
            var semicolonIndex = mediaType!.IndexOf(';');
#if NET6_0_OR_GREATER
            var baseType = semicolonIndex >= 0 ? mediaType[..semicolonIndex].Trim() : mediaType.Trim();
#else
            var baseType = semicolonIndex >= 0 ? mediaType.Substring(0, semicolonIndex).Trim() : mediaType.Trim();
#endif

            // Remove subtype extensions (e.g., "application/problem+json" -> "application/json")
            var plusIndex = baseType.IndexOf('+');
            if (plusIndex >= 0)
            {
#if NET6_0_OR_GREATER
                var lastPart = baseType[(plusIndex + 1)..];
#else
                var lastPart = baseType.Substring(plusIndex + 1);
#endif
                var slashIndex = baseType.IndexOf('/');
                if (slashIndex >= 0)
                {
#if NET6_0_OR_GREATER
                    baseType = string.Concat(baseType[..(slashIndex + 1)], lastPart);
#else
                    baseType = string.Concat(baseType.Substring(0, slashIndex + 1), lastPart);
#endif
                }
            }

            return baseType;
        }

        private IContentSerializer? _getSerializer(string? baseMediaType)
        {
            if (string.IsNullOrEmpty(baseMediaType))
                return null;

            return _serializers.FirstOrDefault(s => string.Equals(s.MediaType, baseMediaType, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Exec(HttpMethod method, string resource, CancellationToken ctk = default)
            => await Exec<object?, object?>(method, resource, default, ctk).ConfigureAwait(false);

        public Task<TResult> Exec<TResult>(HttpMethod method, string resource, CancellationToken ctk = default)
            => Exec<TResult, object?>(method, resource, default, ctk);

        public async Task Exec<TBody>(HttpMethod method, string resource, TBody body, CancellationToken ctk = default)
            => await Exec<object, TBody>(method, resource, body, ctk).ConfigureAwait(false);
    }
    /// <summary>
    /// Flurl Extension
    /// </summary>
    internal static class FlurlExt
    {
        /// <summary>
        /// Flurl request extension to return Accept headers
        /// </summary>
        /// <param name="request">IFlurlRequest</param>
        /// <param name="serializers">List of content serializers</param>
        /// <returns></returns>
        internal static IFlurlRequest WithAcceptHeader(this IFlurlRequest request, List<IContentSerializer> serializers)
        {
            var cnt = serializers.Count;
            var step = 1.0 / (cnt + 1);
            var sb = new StringBuilder();
            var headers = serializers.Select((x, i) => new MediaTypeWithQualityHeaderValue(x.MediaType, 1 - (step * i)));

            return request.WithHeader("Accept", string.Join(",", headers));
        }
    }
}
