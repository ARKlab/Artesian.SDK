using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Principals entity
    /// </summary>
    [MessagePackObject]
    public record Principals
    {
        /// <summary>
        /// The Principals name
        /// </summary>
        [Key("Principal")]
        public required string Principal { get; init; }
        /// <summary>
        /// The Principals type
        /// </summary>
        [Key("Type")]
        public required string Type { get; init; }
    }
}
