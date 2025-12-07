using Artesian.SDK.Dto.UoM;

using MessagePack;

using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operation for Update the UnitOfMeasure
    /// </summary>
    [MessagePackObject]
    public record OperationUpdateUnitOfMeasure : IOperationParamsPayload
    {
        /// <summary>
        /// Value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public required UnitOfMeasure Value { get; init; }
    }
}
