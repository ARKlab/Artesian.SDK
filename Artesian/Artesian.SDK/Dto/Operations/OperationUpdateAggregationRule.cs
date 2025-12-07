using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operation for Update Aggregation rule
    /// </summary>
    [MessagePackObject]
    public record OperationUpdateAggregationRule : IOperationParamsPayload
    {
        /// <summary>
        /// The AggregationRule Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public AggregationRule Value { get; init; }
    }
}
