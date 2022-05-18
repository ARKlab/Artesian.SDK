// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Actual Query Paramaters DTO
    /// </summary>
    public class ActualQueryParamaters : QueryWithFillAndIntervalParamaters 
    {
        /// <summary>
        /// 
        /// </summary>
        public ActualQueryParamaters()
        {

        }

        /// <summary>
        /// Actual Query Paramaters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="granularity"></param>
        /// <param name="transformId"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerC"></param>
        /// <param name="fillerConfig"></param>
        public ActualQueryParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            Granularity? granularity, 
            int? transformId,
            FillerKindType fillerK,
            bool fillerC,
            FillerConfig fillerConfig
            )
            : base(ids,extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK, fillerC, fillerConfig)
        {           
            this.Granularity = granularity;
            this.TransformId = transformId;
            this.FillerConfig = fillerConfig;
        }

        /// <summary>
        /// Granularity
        /// </summary>
        public Granularity? Granularity { get; set; }
        /// <summary>
        /// Time range
        /// </summary>
        public int? TransformId { get; set; }
    }
}
