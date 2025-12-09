using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AclPath entity
    /// </summary>
    [MessagePackObject]
    public record AclPath
    {
        /// <summary>
        /// The Acl Path
        /// </summary>
        [Key(0)]
        public required string Path { get; init; }
        
        /// <summary>
        /// The AclPath ETag
        /// </summary>
        [Key(1)]
        public string? ETag { get; init; }
        
        /// <summary>
        /// The AclPathRule list
        /// </summary>
        [Key(2)]
        public List<AclPathRule> Roles { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        [SerializationConstructor]
        public AclPath()
        {
            Roles = new List<AclPathRule>();
        }
    }
}
