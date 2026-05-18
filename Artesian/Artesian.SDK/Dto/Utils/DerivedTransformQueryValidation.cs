namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Derived transform query validation.
    /// </summary>
    public class DerivedTransformQueryValidation
    {
        /// <summary>
        /// V1
        /// </summary>
        public record V1
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="V1"/> record.
            /// </summary>
            public V1()
            {
                Data = new TimeSerieData.V1();
            }

            /// <summary>
            /// The SQL-like transform query to apply on the referenced time series.
            /// </summary>
            /// <remarks>
            /// The query is executed against the helper table <c>$table</c>.
            /// For <b>ActualTimeSerie</b>, the table exposes <c>Time</c> (datetime) and <c>Value</c> (double).
            /// For <b>VersionedTimeSerie</b>, it exposes <c>Version</c> (datetime), <c>Time</c> (datetime), and <c>Value</c> (double).
            /// The transform query should return <c>Time</c> and <c>Value</c> columns in the response.
            /// </remarks>
            /// <example>
            /// Example queries:
            /// SELECT Time + INTERVAL 1 DAY AS Time, Value FROM $table
            /// SELECT Time, CASE WHEN EXTRACT(HOUR FROM (Time AT TIME ZONE 'UTC') AT TIME ZONE 'Europe/Rome') &lt; 10 THEN Value + 1 ELSE Value END AS Value FROM $table WHERE Time IS NOT NULL
            /// SELECT Time, Value FROM $table WHERE Version IS NOT NULL AND ((EXTRACT(hour FROM Version) &lt; 10 AND Time &gt;= date_trunc('day', Version + interval '1 day')) OR (EXTRACT(hour FROM Version) &gt;= 10 AND Time &gt;= date_trunc('day', Version + interval '2 day')))
            /// </example>
            public string? Transform { get; init; }

            /// <summary>
            /// The time series data used for the query validation.
            /// </summary>
            public TimeSerieData.V1 Data { get; set; }
        }
    }
}
