// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Constants
    /// </summary>
    public abstract class ArtesianConstants
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public readonly static string SDKVersionHeaderValue = $@"ArtesianSDK-C#:{Assembly.GetExecutingAssembly().GetName().Version},{Environment.OSVersion.Platform}:{Environment.OSVersion.Version},{_frameworkName?.Identifier}:{_frameworkName?.Version}";
 #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		internal const string CharacterValidatorRegEx = @"^[^'"",:;\s](?:(?:[^'"",:;\s]| )*[^'"",:;\s])?$";
        internal const string MarketDataNameValidatorRegEx = @"^[^\s](?:(?:[^\s]| )*[^\s])?$";
        internal const string QueryVersion = "v1.0";
        internal const string GMEPublicOfferVersion = "v2.0";
        internal const string QueryRoute = "query";
        internal const string GMEPublicOfferRoute = "gmepublicoffer";
        internal const string MetadataVersion = "v2.1";
        internal const int ServiceRequestTimeOutMinutes = 10;

        private static FrameworkName _frameworkName { get => new FrameworkName(Assembly.GetExecutingAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName); }
    }
}
