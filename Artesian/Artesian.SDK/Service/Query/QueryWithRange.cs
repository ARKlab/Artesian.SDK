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
        protected readonly TQueryParams _queryParamaters = new TQueryParams();

        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;
        private static LocalDateTimePattern _localDateTimePattern = LocalDateTimePattern.ExtendedIso;
        
        /// <summary>
        /// Set the marketData id to be queried
        /// </summary>
        /// <param name="ids">Int[]</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _forMarketData(int[] ids)
        {
            _queryParamaters.FilterId = null;

            _queryParamaters.Ids = ids;
            return this;
        }
        /// <summary>
        /// Set the filter id to be queried
        /// </summary>
        /// <param name="filterId">The filter id to be queried</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _forFilterId(int filterId)
        {
            _queryParamaters.Ids = null;

            _queryParamaters.FilterId = filterId;
            return this;
        }
        /// <summary>
        /// Set the timezone to be queried
        /// </summary>
        /// <param name="tz">String timezone in IANA format</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _inTimezone(string tz)
        {
            if (DateTimeZoneProviders.Tzdb.GetZoneOrNull(tz) == null)
                throw new ArgumentException($"Timezone {tz} is not recognized");
            _queryParamaters.TimeZone = tz;
            return this;
        }

        /// <summary>
        /// Query by absolute range
        /// </summary>
        /// <param name="start">Local date Start</param>
        /// <param name="end">Local date End</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _inAbsoluteDateRange(LocalDate start, LocalDate end)
        {
            if (end <= start)
                throw new ArgumentException("End date " + end + " must be greater than start date " + start);

            _queryParamaters.ExtractionRangeType = ExtractionRangeType.DateRange;
            _queryParamaters.ExtractionRangeSelectionConfig.DateStart = start;
            _queryParamaters.ExtractionRangeSelectionConfig.DateEnd = end;
            return this;
        }

        /// <summary>
        /// Query by relative period range
        /// </summary>
        /// <param name="from">Period Start</param>
        /// <param name="to">Period End</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _inRelativePeriodRange(Period from, Period to)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.PeriodRange;
            _queryParamaters.ExtractionRangeSelectionConfig.PeriodFrom = from;
            _queryParamaters.ExtractionRangeSelectionConfig.PeriodTo = to;
            return this;
        }

        /// <summary>
        /// Query by relative period
        /// </summary>
        /// <param name="extractionPeriod">Period</param>
        /// <returns>Query</returns>
        protected QueryWithRange<TQueryParams> _inRelativePeriod(Period extractionPeriod)
        {
            _queryParamaters.ExtractionRangeType = ExtractionRangeType.Period;
            _queryParamaters.ExtractionRangeSelectionConfig.Period = extractionPeriod;
            return this;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        /// <returns></returns>
        protected virtual void _validateQuery()
        {
            if (_queryParamaters.ExtractionRangeType == null)
                throw new ArtesianSdkClientException("Data extraction range must be provided. Provide a date range , period or period range or an interval eg .InAbsoluteDateRange()");

            if (_queryParamaters.Ids == null && _queryParamaters.FilterId == null)
                throw new ArtesianSdkClientException("Marketadata ids OR filterId must be provided for extraction. Use .ForMarketData() OR .ForFilterId() and provide an integer or integer array as an argument");

            if (_queryParamaters.Ids != null && _queryParamaters.FilterId != null)
                throw new ArtesianSdkClientException("Marketadata ids AND filterId cannot be valorized at same time, choose one");
        }

        internal string _toUrlParam(LocalDate start, LocalDate end)
        {
            return $"{_localDatePattern.Format(start)}/{_localDatePattern.Format(end)}";
        }

        internal string _toUrlParam(LocalDateTime start, LocalDateTime end)
        {
            return $"{_localDateTimePattern.Format(start)}/{_localDateTimePattern.Format(end)}";
        }

        internal string _toUrlParam(LocalDateTime dateTime)
        {
            return _localDateTimePattern.Format(dateTime);
        }

    }
}