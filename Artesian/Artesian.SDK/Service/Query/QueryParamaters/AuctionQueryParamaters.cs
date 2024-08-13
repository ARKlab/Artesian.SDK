// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Auction Query Paramaters DTO
    /// </summary>
    public class AuctionQueryParamaters : QueryWithRangeParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        public AuctionQueryParamaters()
        {

        }

        /// <summary>
        /// Auction Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        public AuctionQueryParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId
            )
            : base(ids,extractionRangeSelectionConfig, extractionRangeType, timezone, filterId)
        {           
        }
    }
}
