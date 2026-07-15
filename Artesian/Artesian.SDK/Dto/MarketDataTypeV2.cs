namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents the supported market data time series types.
    /// </summary>
    public enum MarketDataTypeV2
    {
        /// <summary>
        /// Actual time series market data type.
        /// </summary>
        ActualTimeSerie
        , /// <summary>
          /// Versioned time series market data type.
          /// </summary>
        VersionedTimeSerie
        , /// <summary>
          /// Market assessment market data type.
          /// </summary>
        MarketAssessment
        , /// <summary>
          /// Auction market data type.
          /// </summary>
        Auction
        , /// <summary>
          /// Bid/ask market data type.
          /// </summary>
        BidAsk
    }
}
