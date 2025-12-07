using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Authorization Path entity
    /// </summary>
    public static class AuthorizationPath
    {
        /// <summary>
        /// The Authorization Path entity Input
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The Authorization Path
            /// </summary>
            [Key(0)]
            public string Path { get; set; } = string.Empty;
            /// <summary>
            /// The Authorization Roles related
            /// </summary>
            [Key(1)]
            public IEnumerable<AuthorizationPrincipalRole> Roles { get; set; } = new List<AuthorizationPrincipalRole>();
        }

        /// <summary>
        /// The Authorization Path entity Output
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
        }
    }
}

