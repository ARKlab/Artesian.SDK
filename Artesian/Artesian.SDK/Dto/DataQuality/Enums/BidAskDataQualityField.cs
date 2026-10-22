namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Identifies a BidAsk field selected by a Data Quality rule.
    /// </summary>
    public enum BidAskDataQualityField
    {
        /// <summary>No BidAsk field was selected.</summary>
        Unspecified = 0,

        /// <summary>The best bid price.</summary>
        BestBidPrice = 1,

        /// <summary>The best ask price.</summary>
        BestAskPrice = 2,

        /// <summary>The best bid quantity.</summary>
        BestBidQuantity = 3,

        /// <summary>The best ask quantity.</summary>
        BestAskQuantity = 4,

        /// <summary>The last traded price.</summary>
        LastPrice = 5,

        /// <summary>The last traded quantity.</summary>
        LastQuantity = 6
    }
}