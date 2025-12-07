using MessagePack;

using NodaTime;

using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The ApiKey Entity with Etag
    /// </summary>
    public static class ApiKey
    {
        /// <summary>
        /// The ApiKey Entity Input
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The ApiKey Id
            /// </summary>
            [Key(0)]
            public int Id { get; set; }
            /// <summary>
            /// The ApiKey ETag
            /// </summary>
            [Key(1)]
            public string? ETag { get; set; }
            /// <summary>
            /// The ApiKey UsagePerDay
            /// </summary>
            [Key(2)]
            public int? UsagePerDay { get; set; }
            /// <summary>
            /// The expiration time of the ApiKey
            /// </summary>
            [Key(3)]
            public Instant? ExpiresAt { get; set; }
            /// <summary>
            /// Desctiption
            /// </summary>
            [Key(4)]
            public string? Description { get; set; }

        }

        /// <summary>
        /// The ApiKey Entity Output
        /// </summary>
        [MessagePackObject]
        public class Output
        {
            /// <summary>
            /// The ApiKey Id
            /// </summary>
            [Key(0)]
            public int Id { get; set; }
            /// <summary>
            /// The ApiKey ETag
            /// </summary>
            [Key(1)]
            public string? ETag { get; set; }
            /// <summary>
            /// The ApiKey UsagePerDay
            /// </summary>
            [Key(2)]
            public int? UsagePerDay { get; set; }
            /// <summary>
            /// The expire time of ApiKey
            /// </summary>
            [Key(3)]
            public Instant? ExpiresAt { get; set; }
            /// <summary>
            /// Desctiption
            /// </summary>
            [Key(4)]
            public string? Description { get; set; }
            /// <summary>
            /// The ApiKey UserId
            /// </summary>
            [Key(5)]
            public string? UserId { get; set; }
            /// <summary>
            /// The ApiKey Key
            /// </summary>
            [Key(6)]
            public string? Key { get; set; }
            /// <summary>
            /// The Creation time of ApiKey
            /// </summary>
            [Key(7)]
            public Instant CreatedAt { get; set; }
        }
    }

    internal static class ApiKeyExt
    {
        public static void Validate(this ApiKey.Input apiKey)
        {
            if (apiKey.Id != 0)
                throw new ArgumentException("ApiKey Id must be 0", nameof(apiKey));
        }
    }
}
