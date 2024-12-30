// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;
using NodaTime.Text;
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query with range class
    /// </summary>
    public abstract class QueryWithRange<TQueryParams> where TQueryParams : QueryWithRangeParamaters, new()
    {
        /// <summary>
        /// Store for QueryParams
        /// </summary>
        protected TQueryParams QueryParamaters { get; } = new TQueryParams();

        private static readonly LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static readonly LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.ExtendedIso;
        
        /// <summary>
        /// Set the marketData id to be queried
        /// </summary>
        /// <param name="ids">Int[]</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> ForMarketData(int[] ids)
        {
            QueryParamaters.FilterId = null;

            QueryParamaters.Ids = ids;
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> ForFilterId(int filterId)
        {
            QueryParamaters.Ids = null;

            QueryParamaters.FilterId = filterId;
            return this;
        }
        /// <summary>
        /// Set the timezone to be queried
        /// </summary>
        /// <param name="tz">String timezone in IANA format</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> InTimezone(string tz)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(tz) == null)
                throw new ArgumentException($"Timezone {tz} is not recognized", nameof(tz));
            QueryParamaters.TimeZone = tz;
            return this;
        }

        /// <summary>
        /// Query by absolute range
        /// </summary>
        /// <param name="start">Local date Start</param>
        /// <param name="end">Local date End</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> InAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start, nameof(end));

            QueryParamaters.ExtractionRangeType = ExtractionRangeType.DateRange;
            QueryParamaters.ExtractionRangeSelectionConfig.DateStart = start;
            QueryParamaters.ExtractionRangeSelectionConfig.DateEnd = end;
            return this;
        }

        /// <summary>
        /// Query by relative period range
        /// </summary>
        /// <param name="from">Period Start</param>
        /// <param name="to">Period End</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> InRelativePeriodRange(Period from, Period to)
        {
            QueryParamaters.ExtractionRangeType = ExtractionRangeType.PeriodRange;
            QueryParamaters.ExtractionRangeSelectionConfig.PeriodFrom = from;
            QueryParamaters.ExtractionRangeSelectionConfig.PeriodTo = to;
            return this;
        }

        /// <summary>
        /// Query by relative period
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> InRelativePeriod(Period extractionPeriod)
        {
            QueryParamaters.ExtractionRangeType = ExtractionRangeType.Period;
            QueryParamaters.ExtractionRangeSelectionConfig.Period = extractionPeriod;
            return this;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        /// <returns></returns>
        protected virtual void ValidateQuery()
        {
            if (QueryParamaters.ExtractionRangeType == null)
                throw new ArtesianSdkClientException("Data extraction range must be provided. Provide a date range , period or period range or an interval eg .InAbsoluteDateRange()");

            if (QueryParamaters.Ids == null && QueryParamaters.FilterId == null)
                throw new ArtesianSdkClientException("Marketadata ids OR filterId must be provided for extraction. Use .ForMarketData() OR .ForFilterId() and provide an integer or integer array as an argument");

            if (QueryParamaters.Ids != null && QueryParamaters.FilterId != null)
                throw new ArtesianSdkClientException("Marketadata ids AND filterId cannot be valorized at same time, choose one");
        }

        internal static string ToUrlParam(LocalDate start, LocalDate end)
        {
            return $"{_localDatePattern.Format(start)}/{_localDatePattern.Format(end)}";
        }

        internal static string ToUrlParam(LocalDateTime start, LocalDateTime end)
        {
            return $"{_localDateTimePattern.Format(start)}/{_localDateTimePattern.Format(end)}";
        }

        internal static string ToUrlParam(LocalDateTime dateTime)
        {
            return _localDateTimePattern.Format(dateTime);
        }

    }
}