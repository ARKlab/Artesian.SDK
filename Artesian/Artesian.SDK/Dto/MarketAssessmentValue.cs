// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// Market Assessment Value class
    /// </summary>
    [MessagePackObject]
    public record MarketAssessmentValue
    {
        /// <summary>
        /// Market Assessment Value Construtor by parameters
        /// </summary>
        public MarketAssessmentValue(
            double? settlement = null,
            double? open = null,
            double? close = null,
            double? high = null,
            double? low = null,
            double? volumePaid = null,
            double? volumeGiven = null,
            double? volume = null)
        {
            Settlement = settlement;
            Open = open;
            Close = close;
            High = high;
            Low = low;
            VolumePaid = volumePaid;
            VolumeGiven = volumeGiven;
            Volume = volume;
        }
        /// <summary>
        /// The Market Assessment Settlement
        /// </summary>
        [Key(0)]
        public double? Settlement { get; init; }
        /// <summary>
        /// The Market Assessment Open Price
        /// </summary>
        [Key(1)]
        public double? Open { get; init; }
        /// <summary>
        /// The Market Assessment Close Price
        /// </summary>
        [Key(2)]
        public double? Close { get; init; }
        /// <summary>
        /// The Market Assessment High price
        /// </summary>
        [Key(3)]
        public double? High { get; init; }
        /// <summary>
        /// The Market Assessment Low price
        /// </summary>
        [Key(4)]
        public double? Low { get; init; }
        /// <summary>
        /// The Market Assessment Volume paid
        /// </summary>
        [Key(5)]
        public double? VolumePaid { get; init; }
        /// <summary>
        /// The Market Assessment Volume Given
        /// </summary>
        [Key(6)]
        public double? VolumeGiven { get; init; }
        /// <summary>
        /// The Market Assessment Volume
        /// </summary>
        [Key(7)]
        public double? Volume { get; init; }
    }
}
