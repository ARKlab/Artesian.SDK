using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Absolute-bound outlier configuration for Market Assessment records.
    /// </summary>
    [MessagePackObject]
    public class MasOutlierConfigDto : OutlierConfigDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasOutlierConfigDto"/> class with <see cref="MarketDataType.MarketAssessment"/>.
        /// </summary>
        public MasOutlierConfigDto()
        {
            MarketDataType = MarketDataType.MarketAssessment;
        }

        /// <summary>
        /// Optional products selected by this rule. A null value selects every available product.
        /// </summary>
        [Key("Products")]
        public string[]? Products { get; set; }

        /// <summary>
        /// The Market Assessment fields checked by this rule.
        /// </summary>
        [Key("FieldsToBeValidated")]
        public required MasDataQualityField[] FieldsToBeValidated { get; set; }
    }
}