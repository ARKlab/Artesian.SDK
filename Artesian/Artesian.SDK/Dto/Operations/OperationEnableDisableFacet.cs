using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The OperationEnableFacet class.
    /// </summary>
    [MessagePackObject]
    public record OperationEnableDisableTag : IOperationParamsPayload
    {
        /// <summary>
        /// The Facet Name.
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public required string TagKey { get; init; }

        /// <summary>
        /// The Facet Value.
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public required string TagValue { get; init; }
    }
}
