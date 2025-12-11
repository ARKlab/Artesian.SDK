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
        public record Input
        {
            /// <summary>
            /// The Authorization Path
            /// </summary>
            [Key(0)]
            public required string Path { get; init; }
            
            /// <summary>
            /// The Authorization Roles related
            /// </summary>
            [Key(1)]
            public IEnumerable<AuthorizationPrincipalRole> Roles { get; init; }

            /// <summary>
            /// Constructor
            /// </summary>
            [SerializationConstructor]
            public Input()
            {
                Roles = new List<AuthorizationPrincipalRole>();
            }
        }

        /// <summary>
        /// The Authorization Path entity Output
        /// </summary>
        [MessagePackObject]
        public record Output : Input
        {
        }
    }
}

