using Artesian.SDK.Dto.Enums;

using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Represents a derived configuration that uses the Sum algorithm.
    /// </summary>
    [MessagePackObject]
    public record DerivedCfgTransform : DerivedCfgWithReferencedIds
    {
        /// <summary>
        /// The Derived Alrghorithm
        /// </summary>
        [IgnoreMember]
        public override DerivedAlgorithm DerivedAlgorithm => DerivedAlgorithm.Transform;

        /// <summary>
        /// SQL transform query executed against the referenced time series.
        /// </summary>
        /// <remarks>
        /// The query is executed against the helper table <c>$table</c>.
        ///
        /// Available columns depend on the referenced time series type:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <c>ActualTimeSerie</c>: exposes <c>Time</c> (DateTime) and <c>Value</c> (double).
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <c>VersionedTimeSerie</c>: exposes <c>Version</c> (DateTime),
        /// <c>Time</c> (DateTime) and <c>Value</c> (double).
        /// </description>
        /// </item>
        /// </list>
        ///
        /// The query must return <c>Time</c> and <c>Value</c> columns.
        /// </remarks>
        /// <example>
        /// Shift all timestamps by one day:
        /// <code>
        /// SELECT Time + INTERVAL 1 DAY AS Time, Value
        /// FROM $table
        /// </code>
        /// </example>
        /// <example>
        /// Increase values before 10:00 (Europe/Rome timezone):
        /// <code>
        /// SELECT Time,
        ///        CASE WHEN EXTRACT(HOUR FROM (Time AT TIME ZONE 'UTC')
        ///                  AT TIME ZONE 'Europe/Rome') &lt; 10
        ///             THEN Value + 1
        ///             ELSE Value
        ///        END AS Value
        /// FROM $table
        /// </code>
        /// </example>
        [Key("Transform")]
        public string? Transform { get; set; }
    }
}
