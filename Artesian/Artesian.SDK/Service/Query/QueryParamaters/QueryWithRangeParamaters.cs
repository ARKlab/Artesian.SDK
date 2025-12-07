// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query with Range Paramaters DTO
    /// </summary>
    public abstract class QueryWithRangeParamaters : QueryParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        protected QueryWithRangeParamaters()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        protected QueryWithRangeParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig 
            extractionRangeSelectionConfig, 
            ExtractionRangeType? 
            extractionRangeType, 
            string timezone, 
            int? filterId
            ) : base(ids,timezone,filterId)
        {
            this.Ids = ids;
            this.ExtractionRangeSelectionConfig = extractionRangeSelectionConfig;
            this.ExtractionRangeType = extractionRangeType;
            this.TimeZone = timezone;
            this.FilterId = filterId;
        }

        /// <summary>
        /// Extraction range config
        /// </summary>
        public ExtractionRangeSelectionConfig ExtractionRangeSelectionConfig { get; set; } = new ExtractionRangeSelectionConfig();
        /// <summary>
        /// Extraction range type
        /// </summary>
        public ExtractionRangeType? ExtractionRangeType { get; set; }

    }
}
