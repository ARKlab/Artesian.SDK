// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Paged Result entity
    /// </summary>
    [MessagePackObject]
    public record PagedResult<T>
    {
        /// <summary>
        /// Page Number
        /// </summary>
        [Key(0)]
        public int Page { get; init; }
        /// <summary>
        /// Page size (number of elements by page)
        /// </summary>
        [Key(1)]
        public int PageSize { get; init; }
        /// <summary>
        ///Number of pages
        /// </summary>
        [Key(2)]
        public long Count { get; init; }
        [SerializationConstructor]
        public PagedResult()
        {
            Data = new List<T>();
        }

        /// <summary>
        /// Indicated if the count is partial
        /// </summary>
        [Key(3)]
        public bool IsCountPartial { get; init; }
        /// <summary>
        ///Data
        /// </summary>
        [Key(4)]
        public IEnumerable<T> Data { get; init; }
    }
}
