using MessagePack;

namespace Artesian.SDK.Dto.GMEPublicOffer
{
    /// <summary>
    /// UnitDto class
    /// </summary>
    [MessagePackObject]
    public record UnitDto
    {
        /// <summary>
        /// Unit Id
        /// </summary>
        [Key(0)]
        public int Id { get; init; }

        /// <summary>
        /// Unit
        /// </summary>
        [Key(1)]
        public string? Unit { get; init; }
    }
}
