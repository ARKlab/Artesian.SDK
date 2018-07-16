﻿namespace Artesian.SDK
{
    public abstract class ArtesianConstants
    {
        internal const string CharacterValidatorRegEx = @"^[^'"",:;\s](?:(?:[^'"",:;\s]| )*[^'"",:;\s])?$";
        internal const string MarketDataNameValidatorRegEx = @"^[^\s](?:(?:[^\s]| )*[^\s])?$";

        internal static string QueryVersion { get { return "v1.0"; } }
        internal static string QueryRoute { get { return "query"; } }

        internal static string MetadataVersion { get { return "v2.1"; } }

    }
}
