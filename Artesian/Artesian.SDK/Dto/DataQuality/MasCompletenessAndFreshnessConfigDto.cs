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