using NodaTime;
using NodaTime.Calendars;

using System;
using System.Runtime.CompilerServices;

namespace Artesian.SDK.Common
{

    /// <summary>
    /// Nodatime Intervals Extensions
    /// </summary>
    internal static class NodatimeIntervalsEx
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static bool IsStartOfInterval(this LocalDateTime localTime, DatePeriod period)
        {
            return localTime.AtStartOfInterval(period) == localTime;
        }

        public static bool IsStartOfInterval(this LocalDateTime localTime, TimePeriod period)
        {
            return localTime.AtStartOfInterval(period) == localTime;
        }

        public static LocalDateTime AtStartOfInterval(this LocalDateTime localTime, TimePeriod period)
        {
            return (localTime.InUtc() - _offsetFromStart(localTime.TimeOfDay, period)).LocalDateTime;
        }

        public static LocalDateTime AtStartOfInterval(this LocalDateTime localTime, DatePeriod period)
        {
            return localTime.Date.AtStartOfInterval(period).AtMidnight();
        }

        private static Duration _offsetFromStart(LocalTime time, TimePeriod period)
        {
            var offset = Duration.Zero;
            offset += Duration.FromTicks(time.TickOfSecond);
            offset += Duration.FromMilliseconds(time.Second);

            switch (period)
            {
                case TimePeriod.Hour:
                    offset += Duration.FromMinutes(time.Minute);
                    break;
                case TimePeriod.Minute:
                    break;
                case TimePeriod.TenMinutes:
                    offset += Duration.FromMinutes(time.Minute % 10);
                    break;
                case TimePeriod.QuarterHour:
                    offset += Duration.FromMinutes(time.Minute % 15);
                    break;
                case TimePeriod.HalfHour:
                    offset += Duration.FromMinutes(time.Minute % 30);
                    break;
            }

            return offset;
        }

        public static bool IsStartOfInterval(this LocalDate date, DatePeriod period)
        {
            return date.AtStartOfInterval(period) == date;
        }

        public static LocalDate AtStartOfInterval(this LocalDate date, DatePeriod period)
        {
            return period switch
            {
                DatePeriod.Day => date,
                DatePeriod.Week => date.FirstDayOfTheWeek(),
                DatePeriod.Month => date.FirstDayOfTheMonth(),
                DatePeriod.Bimestral => new LocalDate(date.Year, ((date.Month - 1) / 2) * 2 + 1, 1, date.Calendar),
                DatePeriod.Trimestral => date.FirstDayOfTheQuarter(),
                DatePeriod.Calendar => date.FirstDayOfTheYear(),
                _ => throw new NotSupportedException("DatePeriod is not supported: " + period.ToString()),
            };
        }

        private static void _ensureIsoCalendar(LocalDate date, [CallerArgumentExpression(nameof(date))] string parameterName = null)
        {
            if (date.Calendar != CalendarSystem.Iso)
                throw new ArgumentException($"LocalDate.Calendar should be CalendarSystem.Iso. Found '{date.Calendar}'", parameterName);
        }

        public static LocalDate FirstDayOfTheWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            _ensureIsoCalendar(date);
            return LocalDate.FromWeekYearWeekAndDay(WeekYearRules.Iso.GetWeekYear(date), WeekYearRules.Iso.GetWeekOfWeekYear(date), dayOfWeek);
        }

        public static LocalDate FirstDayOfTheMonth(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            return new LocalDate(date.Year, date.Month, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheQuarter(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            return new LocalDate(date.Year, (int)((date.Month - 1) / 3) * 3 + 1, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheSeason(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            if (date.Month >= 10)
                return new LocalDate(date.Year, 10, 1, date.Calendar);
            if (date.Month < 4)
                return new LocalDate(date.Year - 1, 10, 1, date.Calendar);

            return new LocalDate(date.Year, 4, 1, date.Calendar);
        }

        public static LocalDate FirstDayOfTheYear(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            return new LocalDate(date.Year, 1, 1, date.Calendar);
        }

        public static LocalDate LastDayOfTheWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Sunday)
        {
            _ensureIsoCalendar(date);
            return LocalDate.FromWeekYearWeekAndDay(WeekYearRules.Iso.GetWeekYear(date), WeekYearRules.Iso.GetWeekOfWeekYear(date), dayOfWeek);
        }

        public static LocalDate LastDayOfTheMonth(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            return date.FirstDayOfTheMonth().PlusMonths(1).Minus(Period.FromDays(1));
        }

        public static LocalDate LastDayOfTheQuarter(this LocalDate date)
        {
            return date.FirstDayOfTheMonth().PlusMonths(3).Minus(Period.FromDays(1));
        }

        public static LocalDate LastDayOfTheSeason(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            if (date.Month >= 10)
                return new LocalDate(date.Year + 1, 3, 31, date.Calendar);
            if (date.Month < 4)
                return new LocalDate(date.Year, 3, 31, date.Calendar);

            return new LocalDate(date.Year, 9, 30, date.Calendar);
        }

        public static LocalDate LastDayOfTheYear(this LocalDate date)
        {
            _ensureIsoCalendar(date);
            return date.FirstDayOfTheYear().PlusYears(1).Minus(Period.FromDays(1));
        }

        public static LocalDate PreviousDayOfWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            _ensureIsoCalendar(date);
            while (date.DayOfWeek != dayOfWeek)
                date = date.PlusDays(-1);

            return date;
        }

        public static LocalDate NextDayOfWeek(this LocalDate date, IsoDayOfWeek dayOfWeek = IsoDayOfWeek.Monday)
        {
            _ensureIsoCalendar(date);
            while (date.DayOfWeek != dayOfWeek)
                date = date.PlusDays(1);

            return date;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
