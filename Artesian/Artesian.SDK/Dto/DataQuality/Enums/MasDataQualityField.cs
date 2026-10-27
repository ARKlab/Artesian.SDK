namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Identifies a Market Assessment field selected by a Data Quality rule.
    /// </summary>
    public enum MasDataQualityField
    {
        /// <summary>No Market Assessment field was selected.</summary>
        Unspecified = 0,

        /// <summary>The settlement value.</summary>
        Settlement = 1,

        /// <summary>The opening value.</summary>
        Open = 2,

        /// <summary>The closing value.</summary>
        Close = 3,

        /// <summary>The highest value.</summary>
        High = 4,

        /// <summary>The lowest value.</summary>
        Low = 5,

        /// <summary>The paid volume.</summary>
        VolumePaid = 6,

        /// <summary>The given volume.</summary>
        VolumeGiven = 7,

        /// <summary>The total volume.</summary>
        Volume = 8
    }
}