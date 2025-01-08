// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.ComponentModel.DataAnnotations;
using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The MarketData id with Etag
    /// </summary>
    [MessagePackObject]
    public record MarketDataETag
    {
        /// <summary>
        /// The MarketData Constructor by id and Etag
        /// </summary>
        public MarketDataETag(int id, string eTag)
        {
            //  Ensure.Bool.IsTrue(id >= ArtesianConstants.CurveIDMin, "id out of accepted Range");
            //  Ensure.Bool.IsTrue(id <= ArtesianConstants.CurveIDMax, "id out of accepted Range");
            if (string.IsNullOrEmpty(eTag))
                throw new ArgumentException("eTag is null or empty", nameof(eTag));

            ID = id;
            ETag = eTag;
        }

        /// <summary>
        /// The MarketData Identifier
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public int ID { get; protected set; }

        /// <summary>
        /// The MarketData ETag
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public string ETag { get; protected set; }

    }
}
