using MessagePack;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// OperatorDto class
    /// </summary>
    [MessagePackObject]
    public record OperatorDto
    {
        /// <summary>
        /// Operator Id
        /// </summary>
        [Key(0)]
        public int Id { get; init; }

        /// <summary>
        /// Operator
        /// </summary>
        [Key(1)]
        public required string Operator { get; init; }
    }
}
