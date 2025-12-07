using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operation for Update Provider Description
    /// </summary>
    [MessagePackObject]
    public record OperationUpdateProviderDescription : IOperationParamsPayload
    {
        /// <summary>
        /// The Provider Description Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public required string Value { get; init; }
    }
}
