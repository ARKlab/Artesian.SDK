using MessagePack;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Configuration for Completeness and Freshness rules applied to actual (non-versioned) time series.
    /// Inherits schedule and record validation settings from <see cref="CompletenessAndFreshnessConfigDto"/>.
    /// </summary>
    [MessagePackObject]
    public class ActualCompletenessAndFreshnessConfigDto : CompletenessAndFreshnessConfigDto
    {
    }
}
