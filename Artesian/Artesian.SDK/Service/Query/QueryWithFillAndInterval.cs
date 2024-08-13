// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query with fill and interval class
    /// </summary>
    public abstract class QueryWithFillAndInterval<TQueryParams>: QueryWithRange<TQueryParams> where TQueryParams : QueryWithFillAndIntervalParamaters , new()
    {        
        /// <summary>
        /// Query by relative interval
        /// </summary>
        /// <param name="relativeInterval">RelativeInterval</param>
        /// <returns>Query</returns>
        protected QueryWithFillAndInterval<TQueryParams> _inRelativeInterval(RelativeInterval relativeInterval)
        {
            QueryParamaters.ExtractionRangeType = ExtractionRangeType.RelativeInterval;
            QueryParamaters.ExtractionRangeSelectionConfig.Interval = relativeInterval;
            return this;
        }

        /// <summary>
        /// Build extraction range
        /// </summary>
        /// <returns>string</returns>
        protected string _buildExtractionRangeRoute(QueryWithFillAndIntervalParamaters  queryParamaters)
        {
            string subPath;
            switch (queryParamaters.ExtractionRangeType)
            {
                case ExtractionRangeType.DateRange:
                    subPath = $"{_toUrlParam(queryParamaters.ExtractionRangeSelectionConfig.DateStart, queryParamaters.ExtractionRangeSelectionConfig.DateEnd)}";
                    break;
                case ExtractionRangeType.Period:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.Period}";
                    break;
                case ExtractionRangeType.PeriodRange:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.PeriodFrom}/{queryParamaters.ExtractionRangeSelectionConfig.PeriodTo}";
                    break;
                case ExtractionRangeType.RelativeInterval:
                    subPath = $"{queryParamaters.ExtractionRangeSelectionConfig.Interval}";
                    break;
                default:
                    throw new NotSupportedException("ExtractionRangeType");
            }

            return subPath;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        /// <returns></returns>
        protected override void _validateQuery()
        {
            base._validateQuery();
        }

    }
}