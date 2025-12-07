using MessagePack;
using System;
using System.Collections.Generic;
using KeyAttribute = MessagePack.KeyAttribute;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The CustomFilter Entity with Etag
    /// </summary>
    [MessagePackObject]
    public record CustomFilter
    {
        /// <summary>
        /// The CustomFilter Id
        /// </summary>
        [Key(0)]
        public int Id { get; init; }
        
        /// <summary>
        /// The CustomFilter Name
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        [Key(1)]
        public required string Name { get; init; }
        
        /// <summary>
        /// The CustomFilter Search Text
        /// </summary>
        [Key(2)]
        public string? SearchText { get; init; }
        
        /// <summary>
        /// The CustomFilter values
        /// </summary>
        [Key(3)]
        public Dictionary<string, List<string>> Filters { get; init; } = new Dictionary<string, List<string>>();
        
        /// <summary>
        /// The CustomFilter Etag
        /// </summary>
        [Key(4)]
        public string? ETag { get; init; }
    }

    internal static class CustomFilterExt
    {
        public static void Validate(this CustomFilter customfilter)
        {
            if (String.IsNullOrWhiteSpace(customfilter.Name))
                throw new ArgumentException("CustomFilter Name must be valorized", nameof(customfilter));

            if (String.IsNullOrWhiteSpace(customfilter.SearchText) && customfilter.Filters == null)
                throw new ArgumentException("Either filter text or filter key values must be provided", nameof(customfilter));
        }
    }
}
