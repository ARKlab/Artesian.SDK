// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;

using NodaTime;

using System;
using System.Runtime.InteropServices;

namespace Artesian.SDK.Dto.DataQuality
{
    /// <summary>
    /// Represents a half-open range of LocalDateTime values [start, end).
    /// </summary>
    [MessagePackObject]
    [StructLayout(LayoutKind.Auto)]
    public readonly struct LocalDateTimeRange : IEquatable<LocalDateTimeRange>
    {
        /// <summary>
        /// Gets the inclusive start of the range.
        /// </summary>
        [Key(0)]
        public LocalDateTime Start { get; init; }

        /// <summary>
        /// Gets the exclusive end of the range.
        /// </summary>
        [Key(1)]
        public LocalDateTime End { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeRange"/> struct.
        /// </summary>
        /// <param name="start">The inclusive start of the range.</param>
        /// <param name="end">The exclusive end of the range.</param>
        public LocalDateTimeRange(LocalDateTime start, LocalDateTime end)
        {
            Start = start;
            End = end;
        }

        /// <inheritdoc/>
        public bool Equals(LocalDateTimeRange other) => Start == other.Start && End == other.End;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is LocalDateTimeRange other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Start.GetHashCode() * 397) ^ End.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether two ranges are equal.
        /// </summary>
        public static bool operator ==(LocalDateTimeRange left, LocalDateTimeRange right) => left.Equals(right);

        /// <summary>
        /// Determines whether two ranges are not equal.
        /// </summary>
        public static bool operator !=(LocalDateTimeRange left, LocalDateTimeRange right) => !left.Equals(right);
    }
}
