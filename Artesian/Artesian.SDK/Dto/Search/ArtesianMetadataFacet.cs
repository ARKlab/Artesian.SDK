// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System.Collections.Generic;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Facet Entity
    /// </summary>
    [MessagePackObject]
    public record ArtesianMetadataFacet
    {
        /// <summary>
        /// Facet Name
        /// </summary>
        [Key(0)]
        public required string FacetName { get; init; }
        /// <summary>
        /// Facet Type
        /// </summary>
        [Key(1)]
        public ArtesianMetadataFacetType FacetType { get; init; }
        /// <summary>
        /// Facet Values
        /// </summary>
        [Key(2)]
        public List<ArtesianMetadataFacetCount> Values { get; init; } = new();
    }

    /// <summary>
    /// Metadata Facet Entity
    /// </summary>
    [MessagePackObject]
    public record ArtesianMetadataFacetCount
    {
        /// <summary>
        /// Metadata Facet Entity Value
        /// </summary>
        [Key(0)]
        public required string Value { get; init; }
        /// <summary>
        /// Metadata Facet Entity Count
        /// </summary>
        [Key(1)]
        public long? Count { get; init; }
    }

    /// <summary>
    /// Metadata Facet Entity Type
    /// </summary>
    public enum ArtesianMetadataFacetType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
          Property = 0
        , Tag
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
