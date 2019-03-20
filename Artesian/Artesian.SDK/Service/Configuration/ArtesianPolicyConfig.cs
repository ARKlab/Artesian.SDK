﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Net.Http;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Policy Config
    /// </summary>
    public class ArtesianPolicyConfig
    {
        private  AsyncCircuitBreakerPolicy _circuitBreakerPolicy { get; set; }

        private  AsyncRetryPolicy _retryPolicy { get; set; }

        private  AsyncBulkheadPolicy _bulkheadPolicy { get; set; }

        private const int _maxParallelism = 10;
        private const int _maxQueuingActions = 15;
        private const int _maxExceptions = 2;
        private const int _retryWaitTime = 200;
        private const int _retryCount = 3;
        private const int _durationOfBreak = 3;
        /// <summary>
        /// Artesian Policy Config
        /// </summary>
        public  ArtesianPolicyConfig()
        {
            RetryPolicyConfig().
            CircuitBreakerPolicyConfig().
            BulkheadPolicyConfig().
            GetResillianceStrategy();
        }
        /// <summary>
        /// Wait and Retry Policy Config
        /// Exponential Backoff strategy
        /// </summary>
        /// <param name="retryCount">Exponential backoff count</param>
        /// <param name="retryWaitTime">Wait time for exponential backoff in milliseconds</param>
        /// <returns></returns>
        public ArtesianPolicyConfig RetryPolicyConfig(int retryCount = _retryCount, int retryWaitTime = _retryWaitTime)
        {
            _retryPolicy = Policy
                .Handle<Exception>(x =>
                {
                    var result = x.InnerException is HttpRequestException;
                    return result;
                })
                .WaitAndRetryAsync(retryCount, retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(retryWaitTime,
                    retryAttempt)
                 ));

            return this;
        }
        /// <summary>
        /// Circuit Breaker Policy
        /// </summary>
        /// <param name="maxExceptions">Max exceptions allowed</param>
        /// <param name="durationOfBreak">Duration of break in seconds</param>
        /// <returns></returns>
        public ArtesianPolicyConfig CircuitBreakerPolicyConfig(int maxExceptions = _maxExceptions, int durationOfBreak = _durationOfBreak)
        {
            _circuitBreakerPolicy = Policy
                .Handle<Exception>(x =>
                {
                    var result = x.InnerException is HttpRequestException;
                    return result;
                })
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
        public ArtesianPolicyConfig BulkheadPolicyConfig(int maxParallelism = _maxParallelism, int maxQueuingActions = _maxQueuingActions)
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

    }
}