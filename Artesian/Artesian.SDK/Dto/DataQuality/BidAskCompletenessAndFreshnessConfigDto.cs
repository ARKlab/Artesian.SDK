using Artesian.SDK.Dto.DataQuality.Enums;

using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Configuration for Completeness and Freshness rules applied to BidAsk records.
    /// </summary>
    [MessagePackObject]
    public class BidAskCompletenessAndFreshnessConfigDto : CompletenessAndFreshnessConfigDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BidAskCompletenessAndFreshnessConfigDto"/> class with <see cref="MarketDataType.BidAsk"/>.
        /// </summary>
        public BidAskCompletenessAndFreshnessConfigDto()
        {
            MarketDataType = MarketDataType.BidAsk;
        }

        /// <summary>
        /// The relative or special products expected at each report time.
        /// </summary>
        [Key("Products")]
        public required string[] Products { get; set; }

        /// <summary>
        /// The BidAsk fields that must be validated.
        /// </summary>
        [Key("FieldsToBeValidated")]
        public required BidAskDataQualityField[] FieldsToBeValidated { get; set; }
    }
}