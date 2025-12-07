using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AclPath entity
    /// </summary>
    [MessagePackObject]
    public class AclPath
    {
        /// <summary>
        /// The Acl Path
        /// </summary>
        [Key(0)]
        public string Path { get; set; }
        /// <summary>
        /// The AclPath ETag
        /// </summary>
        [Key(1)]
        public string ETag { get; set; }
        /// <summary>
        /// The AclPathRule list
        /// </summary>
        [Key(2)]
        public List<AclPathRule>? Roles { get; set; } = new List<AclPathRule>();
    }
}
