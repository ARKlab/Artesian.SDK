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
        public record Input
        {
            /// <summary>
            /// The ApiKey Id
            /// </summary>
            [Key(0)]
            public int Id { get; init; }
            /// <summary>
            /// The ApiKey ETag
            /// </summary>
            [Key(1)]
            public string? ETag { get; init; }
            /// <summary>
            /// The ApiKey UsagePerDay
            /// </summary>
            [Key(2)]
            public int? UsagePerDay { get; init; }
            /// <summary>
            /// The expiration time of the ApiKey
            /// </summary>
            [Key(3)]
            public Instant? ExpiresAt { get; init; }
            /// <summary>
            /// Desctiption
            /// </summary>
            [Key(4)]
            public string? Description { get; init; }

        }

        /// <summary>
        /// The ApiKey Entity Output
        /// </summary>
        [MessagePackObject]
        public record Output
        {
            /// <summary>
            /// The ApiKey Id
            /// </summary>
            [Key(0)]
            public int Id { get; init; }
            /// <summary>
            /// The ApiKey ETag
            /// </summary>
            [Key(1)]
            public string? ETag { get; init; }
            /// <summary>
            /// The ApiKey UsagePerDay
            /// </summary>
            [Key(2)]
            public int? UsagePerDay { get; init; }
            /// <summary>
            /// The expire time of ApiKey
            /// </summary>
            [Key(3)]
            public Instant? ExpiresAt { get; init; }
            /// <summary>
            /// Desctiption
            /// </summary>
            [Key(4)]
            public string? Description { get; init; }
            /// <summary>
            /// The ApiKey UserId
            /// </summary>
            [Key(5)]
            public string? UserId { get; init; }
            /// <summary>
            /// The ApiKey Key
            /// </summary>
            [Key(6)]
            public string? Key { get; init; }
            /// <summary>
            /// The Creation time of ApiKey
            /// </summary>
            [Key(7)]
            public Instant CreatedAt { get; init; }
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
