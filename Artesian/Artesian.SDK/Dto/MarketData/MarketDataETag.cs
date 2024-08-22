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
    public class MarketDataETag
    {
        /// <summary>
        /// The MarketData Constructor by id and Etag
        /// </summary>
        public MarketDataETag(int id, string eTag)
        {
            //  Ensure.Bool.IsTrue(id >= ArtesianConstants.CurveIDMin, "id out of accepted Range");
            //  Ensure.Bool.IsTrue(id <= ArtesianConstants.CurveIDMax, "id out of accepted Range");
            if (string.IsNullOrEmpty(eTag))
                throw new ArgumentException("eTag is null or empty");

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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
        {
            var item = obj as MarketDataETag;
            if (item == null)
            {
                return false;
            }

            return this.ID.Equals(item.ID)
                && this.ETag.Equals(item.ETag, StringComparison.Ordinal);
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode() ^ ETag.GetHashCode();
        }

        public static bool operator ==(MarketDataETag x, MarketDataETag y)
        {
            if (!Equals(x, null))
                return x.Equals(y);
            else if (Equals(y, null))
                return true;
            else
                return false;

        }

        public static bool operator !=(MarketDataETag x, MarketDataETag y)
        {
            if (!Equals(x, null))
                return !x.Equals(y);
            else if (Equals(y, null))
                return false;
            else
                return true;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
