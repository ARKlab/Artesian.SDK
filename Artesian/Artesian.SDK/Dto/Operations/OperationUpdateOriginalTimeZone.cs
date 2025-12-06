using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The OriginalTimeZone Update
    /// </summary>
    [MessagePackObject]
    public class OperationUpdateOriginalTimeZone : IOperationParamsPayload
    {
        /// <summary>
        /// The OriginalTimeZone Update value
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public string Value { get; set; } = null!;

    }
}
