// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Service config Interface
    /// </summary>
    public interface IArtesianServiceConfig
    {
        /// <summary>
        /// Base address of the Artesian service
        /// </summary>
        Uri BaseAddress { get; }
        /// <summary>
        /// Audience of Artesian service. Required when authenticating with Bearer Token
        /// </summary>
        string Audience { get; }
        /// <summary>
        /// IDP Domain. Required when authenticating with Bearer Token
        /// </summary>
        string Domain { get; }
        /// <summary>
        /// Client ID. Required when authenticating with Bearer Token
        /// </summary>
        string ClientId { get; }
        /// <summary>
        /// Client Secret. Required when authenticating with Bearer Token
        /// </summary>
        string ClientSecret { get; }
        /// <summary>
        /// ApiKey used for access to the service
        /// </summary>s
        string ApiKey { get; }
    }
}
