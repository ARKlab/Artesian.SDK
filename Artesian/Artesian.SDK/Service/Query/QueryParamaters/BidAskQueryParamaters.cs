﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// BidAsk Query Paramaters DTO
    /// </summary>
    public class BidAskQueryParamaters : QueryWithFillAndIntervalParamaters 
    {    
        /// <summary>
        /// 
        /// </summary>
        public BidAskQueryParamaters()
        {

        }

        /// <summary>
        /// BidAsk Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="products"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerConfig"></param>
        public BidAskQueryParamaters(
            IEnumerable<int> ids , 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            IEnumerable<string> products,
            FillerKindType fillerK,
            FillerConfig fillerConfig
            )
            : base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK, fillerConfig)
        {
            this.Products = products;
            this.FillerConfig = fillerConfig;
        }
        /// <summary>
        /// Products
        /// </summary>
        public IEnumerable<string> Products { get; set; }
    }
}
