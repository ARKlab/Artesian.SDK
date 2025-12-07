// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Artesian Constants
    /// </summary>
    public static partial class ArtesianConstants
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public readonly static string SDKVersionHeaderValue = $@"ArtesianSDK-C#:{Assembly.GetExecutingAssembly().GetName().Version},{Environment.OSVersion.Platform}:{Environment.OSVersion.Version},{_frameworkName?.Identifier}:{_frameworkName?.Version}";
 #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        internal const string _queryVersion = "v1.0";
        internal const string _gMEPublicOfferVersion = "v2.0";
        internal const string _queryRoute = "query";
        internal const string _gMEPublicOfferRoute = "gmepublicoffer";
        internal const string _metadataVersion = "v2.1";
        internal const int _serviceRequestTimeOutMinutes = 10;

        private static FrameworkName? _frameworkName { get => new FrameworkName(Assembly.GetExecutingAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName); }



        internal const string _characterValidatorRegEx = @"^[^'"",:;\s](?:(?:[^'"",:;\s]| )*[^'"",:;\s])?$";
        internal const string _marketDataNameValidatorRegEx = @"^[^\s](?:(?:[^\s]| )*[^\s])?$";
        internal const string _relativeProductValidatorRegEx = @"\+\d+$";

#if NET8_0_OR_GREATER
        internal static readonly Regex _stringValidator = StringValidatorGenerator();
        internal static readonly Regex _marketDataNameValidator = MarketDataNameValidatorGenerator();
        internal static readonly Regex _relativeProductValidator = RelativeProductValidatorGenerator();

        [GeneratedRegex(_characterValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex StringValidatorGenerator();

        [GeneratedRegex(_marketDataNameValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex MarketDataNameValidatorGenerator();

        [GeneratedRegex(_relativeProductValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex RelativeProductValidatorGenerator();
#else

        internal static readonly Regex _stringValidator = new Regex(_characterValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        internal static readonly Regex _marketDataNameValidator = new Regex(_marketDataNameValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        internal static readonly Regex _relativeProductValidator = new Regex(_relativeProductValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
#endif

    }
}
