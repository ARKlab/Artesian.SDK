using MessagePack;
using System.Collections.Generic;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The AuthGroup entity with Etag
    /// </summary>
    [MessagePackObject]
    public class AuthGroup
    {
        /// <summary>
        /// The AuthGroup constructor
        /// </summary>
        public AuthGroup()
        {
            Name = string.Empty;
            Users = new List<string>();
        }

        /// <summary>
        /// The AuthGroup Identifier
        /// </summary>
        [Key("ID")]
        public int ID { get; set; }
        /// <summary>
        /// The AuthGroup ETag
        /// </summary>
        [Key("ETag")]
        public string? ETag { get; set; }
        /// <summary>
        /// The AuthGroup Name
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [Key("Name")]
        public string Name { get; set; }
        /// <summary>
        /// The AuthGroup Users
        /// </summary>
        [Key("Users")]
        public List<string> Users { get; set; }
    }
}
