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
        [MessagePack.Key(1)]
        public string ETag { get; protected set; }

    }
}
