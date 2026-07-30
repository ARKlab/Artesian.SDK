namespace Artesian.SDK.Dto.DataQuality.Enums
{
    /// <summary>
    /// Specifies the truncation precision applied to the cron-scheduled reference time before
    /// adding Period offsets when computing a completeness/freshness validation window.
    /// When set, the reference time is zeroed to the start of the chosen unit (e.g. Day → midnight),
    /// eliminating schedule-time compensation from Period strings.
    /// When <c>null</c>, no truncation occurs and the current behavior is preserved.
    /// </summary>
    public enum PeriodPrecision
    {
        /// <summary>Truncate to the start of the year: <c>YYYY-01-01 00:00:00</c>.</summary>
        Year = 0,

        /// <summary>Truncate to the start of the month: <c>YYYY-MM-01 00:00:00</c>.</summary>
        Month = 1,

        /// <summary>Truncate to midnight of the date: <c>YYYY-MM-DD 00:00:00</c>.</summary>
        Day = 2,

        /// <summary>Truncate to the start of the hour: <c>YYYY-MM-DD HH:00:00</c>.</summary>
        Hour = 3,

        /// <summary>Truncate to the start of the minute: <c>YYYY-MM-DD HH:MM:00</c>.</summary>
        Minute = 4,

        /// <summary>Full second precision — no truncation.</summary>
        Second = 5
    }
}
