using MessagePack;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The CustomFilter Entity with Etag
    /// </summary>
    [MessagePackObject]
    public class CustomFilter
    {
        /// <summary>
        /// The CustomFilter Id
        /// </summary>
        [Key(0)]
        public int Id { get; set; }
        /// <summary>
        /// The CustomFilter Name
        /// </summary>
        [Key(1)]
        public string? Name { get; set; }
        /// <summary>
        /// The CustomFilter Search Text
        /// </summary>
        [Key(2)]
        public string? SearchText { get; set; }
        /// <summary>
        /// The CustomFilter values
        /// </summary>
        [Key(3)]
        public Dictionary<string, List<string>>? Filters { get; set; }
        /// <summary>
        /// The CustomFilter Etag
        /// </summary>
        [Key(4)]
        public string? ETag { get; set; }
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
