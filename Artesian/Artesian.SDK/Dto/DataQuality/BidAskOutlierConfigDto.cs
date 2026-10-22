using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Absolute-bound outlier configuration for BidAsk records.
    /// </summary>
    [MessagePackObject]
    public class BidAskOutlierConfigDto : OutlierConfigDto
    {
        /// <summary>
        /// Optional products selected by this rule. A null value selects every available product.
        /// </summary>
        [Key("Products")]
        public string[]? Products { get; set; }

        /// <summary>
        /// The BidAsk fields checked by this rule.
        /// </summary>
        [Key("FieldsToBeValidated")]
        public required BidAskDataQualityField[] FieldsToBeValidated { get; set; }
    }
}