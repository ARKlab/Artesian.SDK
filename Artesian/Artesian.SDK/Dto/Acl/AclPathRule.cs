using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AclPathRule entity
    /// </summary>
    [MessagePackObject]
    public class AclPathRule
    {
        /// <summary>
        /// The Acl Principal
        /// </summary>
        [Key(0)]
        public Principal Principal { get; set; } = new Principal();
        /// <summary>
        /// The Acl Role
        /// </summary>
        [Key(1)]
        public string Role { get; set; } = string.Empty;
    }
}
