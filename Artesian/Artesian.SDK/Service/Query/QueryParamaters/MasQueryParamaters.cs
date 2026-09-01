// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Mas Query Paramaters DTO
    /// </summary>
    public class MasQueryParamaters : QueryWithFillAndIntervalParamaters 
    {    
        /// <summary>
        /// 
        /// </summary>
        public MasQueryParamaters()
        {

        }

        /// <summary>
        /// Mas Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="products"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerConfig"></param>
        /// <param name="includeOverrideDetails"></param>
        /// <param name="skipOverrides"></param>
        public MasQueryParamaters(
            IEnumerable<int> ids , 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string? timezone,
            int? filterId,
            IEnumerable<string>? products,
            FillerKindType fillerK,
            FillerConfig fillerConfig,
            bool includeOverrideDetails = false,
            bool skipOverrides = false
            )
            : base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK, fillerConfig)
        {
            this.Products = products;
            this.FillerConfig = fillerConfig;
            this.IncludeOverrideDetails = includeOverrideDetails;
            this.SkipOverrides = skipOverrides;
        }
        /// <summary>
        /// Products
        /// </summary>
        public IEnumerable<string>? Products { get; set; }
        /// <summary>
        /// Whether to include override and fallback details
        /// </summary>
        public bool IncludeOverrideDetails { get; set; }
        /// <summary>
        /// Whether to skip overrides and fallbacks
        /// </summary>
        public bool SkipOverrides { get; set; }
    }
}
