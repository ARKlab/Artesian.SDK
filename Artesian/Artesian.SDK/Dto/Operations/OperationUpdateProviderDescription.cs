using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operation for Update Provider Description
    /// </summary>
    [MessagePackObject]
    public class OperationUpdateProviderDescription : IOperationParamsPayload
    {
        /// <summary>
        /// The Provider Description Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public string Value { get; set; } = null!;

    }
}
