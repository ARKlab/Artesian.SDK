using MessagePack;
using System.Collections.Generic;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AuthGroup entity with Etag
    /// </summary>
    [MessagePackObject]
    public record AuthGroup
    {
        /// <summary>
        /// The AuthGroup Identifier
        /// </summary>
        [Key("ID")]
        public int ID { get; init; }
        
        /// <summary>
        /// The AuthGroup ETag
        /// </summary>
        [Key("ETag")]
        public string? ETag { get; init; }
        
        /// <summary>
        /// The AuthGroup Name
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [Key("Name")]
        public required string Name { get; init; }
        
        /// <summary>
        /// The AuthGroup Users
        /// </summary>
        [Key("Users")]
        public List<string> Users { get; init; } = new List<string>();
    }
}
