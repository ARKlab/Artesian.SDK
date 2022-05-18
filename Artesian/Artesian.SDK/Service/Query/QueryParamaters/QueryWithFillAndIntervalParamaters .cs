// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query with Fill and Interval Paramaters DTO
    /// </summary>
    public abstract class QueryWithFillAndIntervalParamaters : QueryWithRangeParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryWithFillAndIntervalParamaters ()
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
        /// <param name="fillerKind"></param>
        /// <param name="fillerContinue"></param>
        /// <param name="fillerConfig"></param>
        public QueryWithFillAndIntervalParamaters (
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig 
            extractionRangeSelectionConfig, 
            ExtractionRangeType? 
            extractionRangeType, 
            string timezone, 
            int? filterId,
            FillerKindType fillerKind,
            bool fillerContinue,
            FillerConfig fillerConfig
            ): base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId)
        {
            this.FillerKindType = fillerKind;
            this.FillerContinue = fillerContinue;
            this.FillerConfig = fillerConfig;
        }

        /// <summary>
        /// Filler Kind
        /// </summary>
        public FillerKindType FillerKindType { get; set; } = FillerKindType.Default;
        /// <summary>
        /// Filler Continue if no end value
        /// </summary>
        public bool FillerContinue { get; set; }
        /// <summary>
        /// Filler config
        /// </summary>
        public FillerConfig FillerConfig { get; set; } = new FillerConfig();
    }
}
