using NodaTime;

using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Timeserie data
    /// </summary>
    public class TimeSerieData
    {
        /// <summary>
        /// V1
        /// </summary>
        public record V1
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="V1"/> record,
            /// setting up the <see cref="Rows"/> dictionary for time series data.
            /// </summary>
            public V1()
            {
                Rows = new Dictionary<LocalDateTime, double?>();
            }

            /// <summary>
            /// The timeserie data in OriginalTimezone or, when Hourly, UTC.
            /// </summary>
            /// 
            public IDictionary<LocalDateTime, double?> Rows { get; set; }
            /// <summary>
            /// MarketDataEntity Type
            /// </summary>
            public MarketDataType Type { get; set; }
            /// <summary>
            /// The Version to operate on
            /// </summary>
            public LocalDateTime? Version { get; set; }
            /// <summary>
            /// The timezone of the Rows. Must be the OriginalTimezone or, when Hourly, must be "UTC".
            /// </summary>
            public string? Timezone { get; set; }
        }
    }
}
