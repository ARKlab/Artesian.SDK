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
        /// Initializes a new instance of the <see cref="BidAskOutlierConfigDto"/> class with <see cref="MarketDataType.BidAsk"/>.
        /// </summary>
        public BidAskOutlierConfigDto()
        {
            MarketDataType = MarketDataType.BidAsk;
        }

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