using Artesian.SDK.Dto;
using NodaTime;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Filler configuration
    /// </summary>
    public class FillerConfig
    {
        /// <summary>
        /// Filler Default Value
        /// </summary>
        public double? FillerTimeSeriesDV { get; set; }
        /// <summary>
        /// Filler MAS Default Value
        /// </summary>
        public MarketAssessmentValue FillerMasDV { get; set; } = new MarketAssessmentValue();
        /// <summary>
        /// Filler Bid Ask Default Value
        /// </summary>
        public BidAskValue FillerBidAskDV { get; set; } = new BidAskValue();
        /// <summary>
        /// Filler Period
        /// </summary>
        public Period FillerPeriod { get; set; }
    }
}
