using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AclPathRule entity
    /// </summary>
    [MessagePackObject]
    public record AclPathRule
    {
        /// <summary>
        /// The Acl Principal
        /// </summary>
        [Key(0)]
        public required Principal Principal { get; init; }
        
        /// <summary>
        /// The Acl Role
        /// </summary>
        [Key(1)]
        public required string Role { get; init; }
    }
}
