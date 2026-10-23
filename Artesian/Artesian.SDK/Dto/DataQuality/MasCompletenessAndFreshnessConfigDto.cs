using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Configuration for Completeness and Freshness rules applied to Market Assessment records.
    /// </summary>
    [MessagePackObject]
    public class MasCompletenessAndFreshnessConfigDto : CompletenessAndFreshnessConfigDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasCompletenessAndFreshnessConfigDto"/> class with <see cref="MarketDataType.MarketAssessment"/>.
        /// </summary>
        public MasCompletenessAndFreshnessConfigDto()
        {
            MarketDataType = MarketDataType.MarketAssessment;
        }

        /// <summary>
        /// The relative or special products expected at each report time.
        /// </summary>
        [Key("Products")]
        public required string[] Products { get; set; }

        /// <summary>
        /// The Market Assessment fields that must be validated.
        /// </summary>
        [Key("FieldsToBeValidated")]
        public required MasDataQualityField[] FieldsToBeValidated { get; set; }
    }
}