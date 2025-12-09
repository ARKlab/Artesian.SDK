// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Common;

using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Policy Config
    /// </summary>
    public class ArtesianPolicyConfig
    {
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncBulkheadPolicy _bulkheadPolicy;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const int MaxParallelismDefault = 10;
        public const int MaxQueuingActionsDefault = 200;
        public const int MaxExceptionsDefault = 2;
        public const int RetryWaitTimeDefault = 200;
        public const int RetryCountDefault = 3;
        public const int DurationOfBreakDefault = 3;
#pragma warning restore CS1591

        /// <summary>
        /// Artesian Policy Config
        /// </summary>
        public ArtesianPolicyConfig()
        {
            RetryPolicyConfig();
            CircuitBreakerPolicyConfig();
            BulkheadPolicyConfig();
        }
        
        /// <summary>
        /// Wait and Retry Policy Config
        /// Exponential Backoff strategy
        /// </summary>
        /// <param name="retryCount">Exponential backoff count</param>
        /// <param name="retryWaitTime">Wait time for exponential backoff in milliseconds</param>
        /// <returns></returns>
        [MemberNotNull(nameof(_retryPolicy))]
        public ArtesianPolicyConfig RetryPolicyConfig(int retryCount = RetryCountDefault, int retryWaitTime = RetryWaitTimeDefault)
        {
            _retryPolicy = Policy
                .Handle<Exception>(x =>
                {
                    // Check if the exception itself is a non-retriable 4xx client error
                    if (x is ArtesianSdkValidationException ||
                        x is ArtesianSdkOptimisticConcurrencyException ||
                        x is ArtesianSdkForbiddenException)
                    {
                        return false;
                    }

                    // Check if it's an AggregateException
                    // Don't retry only if ALL inner exceptions are non-retriable 4xx client errors
                    if (x is AggregateException aggregateException &&
                        aggregateException.InnerExceptions.All(inner =>
                            inner is ArtesianSdkValidationException ||
                            inner is ArtesianSdkOptimisticConcurrencyException ||
                            inner is ArtesianSdkForbiddenException))
                    {
                        return false;
                    }

                    // Retry all other exceptions
                    return true;
                })
                .WaitAndRetryAsync(
                    DecorrelatedJitterBackoff(TimeSpan.FromMilliseconds(retryWaitTime), TimeSpan.FromSeconds(10), retryCount, fastFirst: true)
                );

            return this;
        }
        /// <summary>
        /// Circuit Breaker Policy
        /// </summary>
        /// <param name="maxExceptions">Max exceptions allowed</param>
        /// <param name="durationOfBreak">Duration of break in seconds</param>
        /// <returns></returns>
        [MemberNotNull(nameof(_circuitBreakerPolicy))]
        public ArtesianPolicyConfig CircuitBreakerPolicyConfig(int maxExceptions = MaxExceptionsDefault, int durationOfBreak = DurationOfBreakDefault)
        {
            _circuitBreakerPolicy = Policy
                .Handle<Exception>(x => x.InnerException is HttpRequestException)
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: maxExceptions,
                    durationOfBreak: TimeSpan.FromSeconds(durationOfBreak)
                );

            return this;
        }
        /// <summary>
        /// Bulkhead Policy Config
        /// </summary>
        /// <param name="maxParallelism">Maximum parallelization of executions through the bulkhead</param>
        /// <param name="maxQueuingActions">Maximum number of actions that may be queuing (waiting to acquire an execution slot) at any time</param>
        /// <returns></returns>
        [MemberNotNull(nameof(_bulkheadPolicy))]
        public ArtesianPolicyConfig BulkheadPolicyConfig(int maxParallelism = MaxParallelismDefault, int maxQueuingActions = MaxQueuingActionsDefault)
        {
            _bulkheadPolicy = Policy
                .BulkheadAsync(maxParallelism, maxQueuingActions);

            return this;
        }
        /// <summary>
        /// Policy Resiliance Strategy
        /// </summary>
        /// <returns></returns>
        public AsyncPolicy GetResillianceStrategy()
        {
            return _circuitBreakerPolicy.WrapAsync(_retryPolicy.WrapAsync(_bulkheadPolicy));
        }

        /// <summary>
        /// Generates sleep durations in an jittered manner, making sure to mitigate any correlations.
        /// For example: 117ms, 236ms, 141ms, 424ms, ...
        /// For background, see https://aws.amazon.com/blogs/architecture/exponential-backoff-and-jitter/.
        /// </summary>
        /// <param name="minDelay">The minimum duration value to use for the wait before each retry.</param>
        /// <param name="maxDelay">The maximum duration value to use for the wait before each retry.</param>
        /// <param name="retryCount">The maximum number of retries to use, in addition to the original call.</param>
        /// <param name="seed">An optional <see cref="Random"/> seed to use.
        /// If not specified, will use a shared instance with a random seed, per Microsoft recommendation for maximum randomness.</param>
        /// <param name="fastFirst">Whether the first retry will be immediate or not.</param>
        public static IEnumerable<TimeSpan> DecorrelatedJitterBackoff(TimeSpan minDelay, TimeSpan maxDelay, int retryCount, int? seed = null, bool fastFirst = false)
        {
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay), minDelay, "should be >= 0ms");
            if (maxDelay < minDelay) throw new ArgumentOutOfRangeException(nameof(maxDelay), maxDelay, $"should be >= {minDelay}");
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "should be >= 0");

            if (retryCount == 0)
                return Enumerable.Empty<TimeSpan>();

            return Enumerate(minDelay, maxDelay, retryCount, fastFirst, new ConcurrentRandom(seed));

            IEnumerable<TimeSpan> Enumerate(TimeSpan min, TimeSpan max, int retry, bool fast, ConcurrentRandom random)
            {
                int i = 0;
                if (fast)
                {
                    i++;
                    yield return TimeSpan.Zero;
                }

                // https://github.com/aws-samples/aws-arch-backoff-simulator/blob/master/src/backoff_simulator.py#L45
                // self.sleep = min(self.cap, random.uniform(self.base, self.sleep * 3))

                // Formula avoids hard clamping (which empirically results in a bad distribution)
                double ms = min.TotalMilliseconds;
                for (; i < retry; i++)
                {
                    double ceiling = Math.Min(max.TotalMilliseconds, ms * 3);
                    ms = random.Uniform(min.TotalMilliseconds, ceiling);

                    yield return TimeSpan.FromMilliseconds(ms);
                }
            }
        }
    }
}
