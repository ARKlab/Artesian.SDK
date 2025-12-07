// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;
using NodaTime;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Versioned Query Paramaters DTO
    /// </summary>
    public class VersionedQueryParamaters : QueryWithFillAndIntervalParamaters 
    {
        /// <summary>
        /// 
        /// </summary>
        public VersionedQueryParamaters()
        {

        }

        /// <summary>
        /// Versioned Query Paramters
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="extractionRangeSelectionConfig"></param>
        /// <param name="extractionRangeType"></param>
        /// <param name="versionSelectionConfig"></param>
        /// <param name="versionSelectionType"></param>
        /// <param name="granularity"></param>
        /// <param name="transformId"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        /// <param name="versionLimit"></param>
        /// <param name="fillerK"></param>
        /// <param name="fillerConfig"></param>
        /// <param name="analysisDate"></param>
        /// <param name="unitOfMeasure"></param>
        /// <param name="aggregationRule"></param>
        public VersionedQueryParamaters(
            IEnumerable<int> ids, 
            ExtractionRangeSelectionConfig extractionRangeSelectionConfig, 
            ExtractionRangeType? extractionRangeType,
            string timezone,
            int? filterId,
            Granularity? granularity,
            int? transformId,
            VersionSelectionConfig versionSelectionConfig,
            VersionSelectionType? versionSelectionType,
            LocalDateTime? versionLimit,
            FillerKindType fillerK,
            FillerConfig fillerConfig,
            LocalDate? analysisDate,
            string unitOfMeasure,
            AggregationRule? aggregationRule
            )
            : base(ids, extractionRangeSelectionConfig, extractionRangeType, timezone, filterId, fillerK, fillerConfig)
        {
            this.VersionSelectionConfig = versionSelectionConfig;
            this.VersionSelectionType = versionSelectionType;
            this.Granularity = granularity;
            this.TransformId = transformId;
            this.VersionLimit = versionLimit;
            this.AnalysisDate = analysisDate;
            this.FillerConfig = fillerConfig;
            this.UnitOfMeasure = unitOfMeasure;
            this.AggregationRule = aggregationRule;
        }

        /// <summary>
        /// Version selection config
        /// </summary>
        public VersionSelectionConfig? VersionSelectionConfig { get; set; } = new VersionSelectionConfig();
        /// <summary>
        /// Version selection type
        /// </summary>
        public VersionSelectionType? VersionSelectionType { get; set; }
        /// <summary>
        /// Granularity
        /// </summary>
        public Granularity? Granularity { get; set; }
        /// <summary>
        /// Time range
        /// </summary>
        public int? TransformId { get; set; }
        /// <summary>
        /// Version Limit
        /// </summary>
        public LocalDateTime? VersionLimit { get; set; }
        /// <summary>
        /// The analysis date from which apply the relative interval (default Today)
        /// </summary>
        public LocalDate? AnalysisDate { get; set; }
        /// <summary>
        /// Unit of Measure
        /// </summary>
        public string UnitOfMeasure { get; set; }
        /// <summary>
        /// AggregationRule
        /// </summary>
        public AggregationRule? AggregationRule { get; set; }
    }
}
