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

        internal const string QueryVersion = "v1.0";
        internal const string GMEPublicOfferVersion = "v2.0";
        internal const string QueryRoute = "query";
        internal const string GMEPublicOfferRoute = "gmepublicoffer";
        internal const string MetadataVersion = "v2.1";
        internal const int ServiceRequestTimeOutMinutes = 10;

        private static FrameworkName _frameworkName { get => new FrameworkName(Assembly.GetExecutingAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName); }



        internal const string CharacterValidatorRegEx = @"^[^'"",:;\s](?:(?:[^'"",:;\s]| )*[^'"",:;\s])?$";
        internal const string MarketDataNameValidatorRegEx = @"^[^\s](?:(?:[^\s]| )*[^\s])?$";
        internal const string RelativeProductValidatorRegEx = @"\+\d+$";

#if NET8_0_OR_GREATER
        internal static readonly Regex StringValidator = StringValidatorGenerator();
        internal static readonly Regex MarketDataNameValidator = MarketDataNameValidatorGenerator();
        internal static readonly Regex RelativeProductValidator = RelativeProductValidatorGenerator();

        [GeneratedRegex(CharacterValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex StringValidatorGenerator();

        [GeneratedRegex(MarketDataNameValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex MarketDataNameValidatorGenerator();

        [GeneratedRegex(RelativeProductValidatorRegEx, RegexOptions.CultureInvariant, 1000)]
        private static partial Regex RelativeProductValidatorGenerator();
#else

        internal static readonly Regex StringValidator = new Regex(CharacterValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        internal static readonly Regex MarketDataNameValidator = new Regex(MarketDataNameValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        internal static readonly Regex RelativeProductValidator = new Regex(RelativeProductValidatorRegEx, RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
#endif

    }
}
