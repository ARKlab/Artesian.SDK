// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// ArtesianServiceConfig
    /// </summary>
    public class ArtesianServiceConfig: IArtesianServiceConfig
    {
        /// <summary>
        /// Base address of the Artesian service
        /// </summary>
        public Uri BaseAddress { get; }
        /// <summary>
        /// Audience of Artesian service. Required when authenticating with Bearer Token
        /// </summary>
        public string Audience { get; }
        /// <summary>
        /// IDP Domain. Required when authenticating with Bearer Token
        /// </summary>
        public string Domain { get; }
        /// <summary>
        /// Client ID. Required when authenticating with Bearer Token
        /// </summary>
        public string ClientId { get; }
        /// <summary>
        /// Client Secret. Required when authenticating with Bearer Token
        /// </summary>
        public string ClientSecret { get; }
        /// <summary>
        /// ApiKey used for access to the service
        /// </summary>s
        public string ApiKey { get; }

        /// <summary>
        /// Config for ApiKey service access
        /// </summary>
        /// <param name="baseAddress">Uri</param>
        /// <param name="xApiKey">String</param>
        public ArtesianServiceConfig(Uri baseAddress, string xApiKey)
        {
            BaseAddress = baseAddress;
            ApiKey = xApiKey;
        }
        /// <summary>
        /// Config for Bearer token service access
        /// </summary>
        /// <param name="baseAddress">Uri</param>
        /// <param name="audience">String</param>
        /// <param name="domain">String</param>
        /// <param name="clientId">String</param>
        /// <param name="clientSecret">String</param>
        public ArtesianServiceConfig(Uri baseAddress, string audience, string domain, string clientId, string clientSecret)
        {
            BaseAddress = baseAddress;
            Audience = audience;
            Domain = domain;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
