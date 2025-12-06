// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;

using Flurl.Http.Testing;

using NodaTime;

using NUnit.Framework;

namespace Artesian.SDK.Tests
{
    /// <summary>
    /// Tests for HTTP content negotiation between client and server
    /// </summary>
    [TestFixture]
    public class ContentNegotiationTests
    {
        private readonly ArtesianServiceConfig _cfg = new ArtesianServiceConfig(new Uri(TestConstants.BaseAddress), TestConstants.APIKey);

        [Test]
        public async Task Should_Accept_All_Supported_MediaTypes_In_Request()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(status: 204);

                var qs = new QueryService(_cfg);

                await qs.CreateActual()
                    .ForMarketData(new[] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().ConfigureAwait(false);

                // Should have all three supported content types in Accept header
                httpTest.ShouldHaveMadeACall()
                    .WithHeader("Accept", "*application/x.msgpacklz4*")
                    .WithHeader("Accept", "*application/x-msgpack*")
                    .WithHeader("Accept", "*application/json*");
            }
        }

        [Test]
        public async Task Should_Deserialize_JSON_Response()
        {
            using (var httpTest = new HttpTest())
            {
                var testData = new TimeSerieRow.Actual
                {
                    TSID = 100000001,
                    Time = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    Value = 42.5
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new[] { testData });

                httpTest.RespondWith(json, 200, headers: new { ContentType = "application/json" });

                var qs = new QueryService(_cfg);

                var result = await qs.CreateActual()
                    .ForMarketData(new[] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().ConfigureAwait(false);

                var resultArray = result.ToArray();
                Assert.That(resultArray, Is.Not.Null);
                Assert.That(resultArray.Length, Is.EqualTo(1));
                Assert.That(resultArray[0].Value, Is.EqualTo(42.5));
            }
        }

        [Test]
        public async Task Should_Handle_JSON_With_Charset_Parameter()
        {
            using (var httpTest = new HttpTest())
            {
                var testData = new TimeSerieRow.Actual
                {
                    TSID = 100000001,
                    Time = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    Value = 42.5
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new[] { testData });

                httpTest.RespondWith(json, 200, headers: new { ContentType = "application/json; charset=utf-8" });

                var qs = new QueryService(_cfg);

                var result = await qs.CreateActual()
                    .ForMarketData(new[] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().ConfigureAwait(false);

                var resultArray = result.ToArray();
                Assert.That(resultArray, Is.Not.Null);
                Assert.That(resultArray.Length, Is.EqualTo(1));
                Assert.That(resultArray[0].Value, Is.EqualTo(42.5));
            }
        }

        [Test]
        public async Task Should_Handle_ProblemDetail_JSON_Response()
        {
            using (var httpTest = new HttpTest())
            {
                var problemDetail = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Bad Request",
                    status = 400,
                    detail = "Invalid request parameters"
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(problemDetail);

                httpTest.RespondWith(json, 400, headers: new { ContentType = "application/problem+json" });

                var qs = new QueryService(_cfg);

                var ex = Assert.ThrowsAsync<ArtesianSdkValidationException>(async () =>
                {
                    await qs.CreateActual()
                        .ForMarketData(new[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().ConfigureAwait(false);
                });

                Assert.That(ex.Message, Does.Contain("Bad Request"));
            }
        }

        [Test]
        public async Task Should_Use_JSON_Serializer_For_ProblemDetail_Extension()
        {
            using (var httpTest = new HttpTest())
            {
                // Problem+JSON should use JSON serializer (extension stripped)
                var problemDetail = new
                {
                    type = "https://example.com/error",
                    title = "Conflict",
                    status = 409,
                    detail = "Resource conflict"
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(problemDetail);

                httpTest.RespondWith(json, 409, headers: new { ContentType = "application/problem+json" });

                var qs = new QueryService(_cfg);

                var ex = Assert.ThrowsAsync<ArtesianSdkOptimisticConcurrencyException>(async () =>
                {
                    await qs.CreateActual()
                        .ForMarketData(new[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().ConfigureAwait(false);
                });

                Assert.That(ex.Message, Does.Contain("Conflict"));
            }
        }

        [Test]
        public void Should_Throw_For_Unsupported_ContentType()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("Some text", 200, headers: new { ContentType = "text/plain" });

                var qs = new QueryService(_cfg);

                var ex = Assert.ThrowsAsync<ArtesianSdkClientException>(async () =>
                {
                    await qs.CreateActual()
                        .ForMarketData(new[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().ConfigureAwait(false);
                });

                Assert.That(ex.Message, Does.Contain("Unsupported content type"));
                Assert.That(ex.Message, Does.Contain("text/plain"));
                Assert.That(ex.Message, Does.Contain("application/json"));
                Assert.That(ex.Message, Does.Contain("application/x-msgpack"));
                Assert.That(ex.Message, Does.Contain("application/x.msgpacklz4"));
            }
        }

        [Test]
        public async Task Should_Handle_Regular_JSON_Error_Response()
        {
            using (var httpTest = new HttpTest())
            {
                // Regular JSON (not problem+json) for error response
                var errorResponse = new { error = "Something went wrong" };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse);

                httpTest.RespondWith(json, 400, headers: new { ContentType = "application/json" });

                var qs = new QueryService(_cfg);

                var ex = Assert.ThrowsAsync<ArtesianSdkValidationException>(async () =>
                {
                    await qs.CreateActual()
                        .ForMarketData(new[] { 100000001 })
                        .InGranularity(Granularity.Day)
                        .InRelativeInterval(RelativeInterval.RollingMonth)
                        .ExecuteAsync().ConfigureAwait(false);
                });

                // Should NOT try to deserialize as ProblemDetail since content type is not "application/problem+json"
                Assert.That(ex, Is.Not.Null);
            }
        }

        [Test]
        public async Task Should_Send_Request_With_LZ4MessagePack_ContentType()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(status: 204);

                var md = new MarketDataService(_cfg);

                var data = new UpsertCurveData()
                {
                    ID = new MarketDataIdentifier("test", "testName"),
                    Timezone = "UTC"
                };

                await md.UpsertCurveDataAsync(data).ConfigureAwait(false);

                var call = httpTest.CallLog.First();
                var contentType = call.Request.Content.Headers.ContentType?.MediaType;

                Assert.That(contentType, Is.EqualTo("application/x.msgpacklz4"));
            }
        }

        [Test]
        public async Task Should_Handle_NoContent_Response()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(status: 204);

                var qs = new QueryService(_cfg);

                var result = await qs.CreateActual()
                    .ForMarketData(new[] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().ConfigureAwait(false);

                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task Should_Handle_NotFound_Response()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(status: 404);

                var qs = new QueryService(_cfg);

                var result = await qs.CreateActual()
                    .ForMarketData(new[] { 100000001 })
                    .InGranularity(Granularity.Day)
                    .InRelativeInterval(RelativeInterval.RollingMonth)
                    .ExecuteAsync().ConfigureAwait(false);

                Assert.That(result, Is.Null);
            }
        }
    }
}
