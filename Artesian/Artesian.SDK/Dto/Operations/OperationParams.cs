using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The OperationParams class.
    /// </summary>
    [MessagePackObject]
    public record OperationParams
    {
        /// <summary>
        /// The Operation type.
        /// </summary>
        [Required]
        [MessagePack.Key("Type")]
        public OperationType Type { get; init; }

        /// <summary>
        /// The Operation specific input.
        /// </summary>
        [Required]
        [MessagePack.Key("Params")]
        public IOperationParamsPayload? Params { get; init; }
    }

    /// <summary>
    /// The Operation Param Payload interface
    /// </summary>
    [Union(0, typeof(OperationEnableDisableTag))]
    [Union(1, typeof(OperationUpdateOriginalTimeZone))]
    [Union(2, typeof(OperationUpdateTimeTransform))]
    [Union(3, typeof(OperationUpdateProviderDescription))]
    [Union(4, typeof(OperationUpdateAggregationRule))]
    [Union(5, typeof(OperationUpdateUnitOfMeasure))]

    public interface IOperationParamsPayload
    { }
    
}
