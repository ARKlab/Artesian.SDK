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
        public string Principal { get; set; } = null!;

        /// <summary>
        /// The Principals type
        /// </summary>
        [Key("Type")]
        public string Type { get; set; } = null!;

    }
}
