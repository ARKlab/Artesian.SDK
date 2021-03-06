﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
namespace Artesian.SDK.Service
{
     enum ExtractionType
    {
        Actual,
        Versioned,
        MAS
    }
    /// <summary>
    /// Extraction range type
    /// </summary>
    public enum ExtractionRangeType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        DateRange,
        Period,
        PeriodRange,
        RelativeInterval
    }
    /// <summary>
    /// Version selection type
    /// </summary>
    public enum VersionSelectionType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        LastN,
        MUV,
        LastOfDays,
        LastOfMonths,
        Version,
        MostRecent
    }
    /// <summary>
    /// Relative interval enums
    /// </summary>
    public enum RelativeInterval
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        RollingWeek,
        RollingMonth,
        RollingQuarter,
        RollingYear,
        WeekToDate,
        MonthToDate,
        QuarterToDate,
        YearToDate
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
    /// <summary>
    /// Filler Kind enums
    /// </summary>
    public enum FillerKindType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Default,
        Null,
        CustomValue,
        LatestValidValue,
        NoFill
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
    /// <summary>
    /// Auction side enums
    /// </summary>
    public enum AuctionSide
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Bid = 0
      , Offer = 1
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
