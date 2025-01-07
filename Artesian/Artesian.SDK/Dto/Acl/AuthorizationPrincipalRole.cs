using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Principal Type Enum
    /// </summary>
    public enum PrincipalType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Group,
        User
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// The Principal Entity
    /// </summary>
    [MessagePackObject]
    public record Principal
    {
        /// <summary>
        /// The Principal Type
        /// </summary>
        [Key(0)]
        public PrincipalType PrincipalType { get; set; }
        /// <summary>
        /// The Principal Identifier
        /// </summary>
        [Key(1)]
        public string PrincipalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Principal()
        {

        }

        private Principal(string s)
        {
            PrincipalId = s.Substring(2);
            PrincipalType = AuthorizationPrincipalRole.DecodePrincipalEnum(s.Substring(0, 1));
        }

        /// <summary>
        /// Encodes the Principal to "g:group" or "u:user"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{AuthorizationPrincipalRole.EncodePrincipalEnum(PrincipalType)}:{PrincipalId}";
        }

        /// <summary>
        /// Implicit conversion from string to Principal
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator string(Principal p) { return p.ToString(); }

        /// <summary>
        /// Implicit conversion from Principal to string
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator Principal(string p) { return new Principal(p); }
    }

    /// <summary>
    /// The AuthorizationPrincipalRole Entity
    /// </summary>
    [MessagePackObject]
    public class AuthorizationPrincipalRole
    {
        /// <summary>
        /// The Role
        /// </summary>
        [Key(0)]
        public string Role { get; set; }
        /// <summary>
        /// The Principal
        /// </summary>
        [Key(1)]
        public Principal Principal { get; set; }
        /// <summary>
        /// The information regarding Inheritance
        /// </summary>
        [Key(2)]
        public string InheritedFrom { get; set; }

        /// <summary>
        /// Encode principal Enum
        /// </summary>
        public static string EncodePrincipalEnum(PrincipalType principalEnum)
        {
            switch (principalEnum)
            {
                case PrincipalType.Group:
                    return "g";
                case PrincipalType.User:
                    return "u";
            }

            throw new InvalidOperationException("unexpected PrincipalType");
        }

        /// <summary>
        /// Decode principal Enum
        /// </summary>
        public static PrincipalType DecodePrincipalEnum(string encoded)
        {
            switch (encoded)
            {
                case "g":
                    return PrincipalType.Group;
                case "u":
                    return PrincipalType.User;
            }

            throw new InvalidOperationException("unexpected encoded string for PrincipalType");
        }
    }
}

