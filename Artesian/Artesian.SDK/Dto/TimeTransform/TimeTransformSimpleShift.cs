// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The TimeTransformSimpleShift entity
    /// </summary>
    [MessagePackObject]
    public class TimeTransformSimpleShift : TimeTransform
    {
        /// <summary>
        /// The Granularity of the Time transform
        /// </summary>
        [Key("Period")]
        public Granularity Period { get; set; }
        /// <summary>
        /// The Positive Shift
        /// </summary>
        [Key(">")]
        public string? PositiveShift { get; set; }
        /// <summary>
        /// The Negative Shift
        /// </summary>
        [Key("<")]
        public string? NegativeShift { get; set; }

        /// <summary>
        /// The Transform Type
        /// </summary>
        [IgnoreMember]
        public override TransformType Type => TransformType.SimpleShift;
    }

    internal static class TimeTransformSimpleShiftExt
    {
        public static void Validate(this TimeTransformSimpleShift timeTransform)
        {
            if (String.IsNullOrWhiteSpace(timeTransform.Name))
                throw new ArgumentException("TimeTransform Name must be valorized", nameof(timeTransform));

            if (String.IsNullOrWhiteSpace(timeTransform.NegativeShift) && String.IsNullOrWhiteSpace(timeTransform.PositiveShift))
                throw new ArgumentException("At least one between positive or negative shift must be valorized", nameof(timeTransform));
        }
    }
}
