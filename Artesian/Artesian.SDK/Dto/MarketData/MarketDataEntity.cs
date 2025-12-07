// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto.UoM;

using MessagePack;

using NodaTime;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The MarketData Entity with Etag
    /// </summary>
    public static class MarketDataEntity
    {
        /// <summary>
        /// The MarketData Entity Input
        /// </summary>
        [MessagePackObject]
        public class Input
        {
            /// <summary>
            /// The MarketData Default Constructor
            /// </summary>
            public Input() { }

            /// <summary>
            /// The MarketData Constructor by MarketDataEntity.Output
            /// </summary>
            public Input(MarketDataEntity.Output output)
            {
                if (output != null)
                {
                    this.MarketDataId = output.MarketDataId;
                    this.ETag = output.ETag;
                    this.ProviderName = output.ProviderName;
                    this.MarketDataName = output.MarketDataName;
                    this.OriginalGranularity = output.OriginalGranularity;
                    this.UnitOfMeasure = output.UnitOfMeasure;
                    this.Type = output.Type;
                    this.OriginalTimezone = output.OriginalTimezone;
                    this.AggregationRule = output.AggregationRule;
                    this.TransformID = output.TransformID;
                    this.ProviderDescription = output.ProviderDescription;
                    this.Tags = output.Tags;
                }
            }

            /// <summary>
            /// The Market Data Identifier
            /// </summary>
            [Required]
            [MessagePack.Key(0)]
            public int MarketDataId { get; set; }
            /// <summary>
            /// The Market Data Etag
            /// </summary>
            [MessagePack.Key(1)]
            public string? ETag { get; set; }
            /// <summary>
            /// The Market Data Provider Name
            /// </summary>
            [Required]
            [MessagePack.Key(2)]
            public string? ProviderName { get; set; }
            /// <summary>
            /// The Market Data Name
            /// </summary>
            [Required]
            [MessagePack.Key(3)]
            public string? MarketDataName { get; set; }
            /// <summary>
            /// The Original Granularity
            /// </summary>
            [Required]
            [MessagePack.Key(4)]
            public Granularity OriginalGranularity { get; set; }
            /// <summary>
            /// The Type
            /// </summary>
            [Required]
            [MessagePack.Key(5)]
            public MarketDataType Type { get; set; }
            /// <summary>
            /// The Original Timezone
            /// </summary>
            [Required]
            [MessagePack.Key(6)]
            public string? OriginalTimezone { get; set; }
            /// <summary>
            /// The Aggregation Rule
            /// </summary>
            [MessagePack.Key(7)]
            public AggregationRule AggregationRule { get; set; }
            /// <summary>
            /// The TimeTransformID
            /// </summary>
            [MessagePack.Key(8)]
            public int? TransformID { get; set; }
            /// <summary>
            /// The Provider description
            /// </summary>
            [MessagePack.Key(9)]
            public string? ProviderDescription { get; set; }
            /// <summary>
            /// The custom Tags assigned to the data
            /// </summary>
            [MessagePack.Key(10)]
            public IDictionary<string, List<string>>? Tags { get; set; }
            /// <summary>
            /// The Authorization Path
            /// </summary>
            [MessagePack.Key(17)]
            public string Path
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(_path))
                        return $@"/marketdata/system/{(ProviderName ?? "").Replace("/", "\\/")}/{(MarketDataName ?? "").Replace("/", "\\/")}";//new PathString(new[] { "marketdata", "system", ProviderName, MarketDataName });
                    return _path;
                }
                set { _path = value; }
            }
            /// <summary>
            /// The Derived Configuration
            /// </summary>
            [MessagePack.Key(18)]
            public DerivedCfgBase DerivedCfg
            {
                get
                {
                    if (_derivedCfg == null)
                    {
                        if (this.Type == MarketDataType.VersionedTimeSerie)
                        {
                            return new DerivedCfgMuv()
                            {
                                Version = 1
                            };
                        }
                        // Return a default configuration if not VersionedTimeSerie
                        return new DerivedCfgMuv() { Version = 1 };
                    }
                    return _derivedCfg;
                }
                set { _derivedCfg = value; }

            }
            /// <summary>
            /// The Unit of Measure
            /// </summary>
            [MessagePack.Key(19)]
            public UnitOfMeasure? UnitOfMeasure { get; set; } = new UnitOfMeasure();

            internal DerivedCfgBase? _derivedCfg;

            internal string? _path;

        }

        /// <summary>
        /// Helpers evaluating path 
        /// </summary>
        public static bool IsPathNull(Input input)
        {
            return string.IsNullOrWhiteSpace(input._path);
        }

        /// <summary>
        /// The MarketData Default Constructor
        /// </summary>
        [MessagePackObject]
        public class Output : Input
        {
            /// <summary>
            /// The MarketData Default Constructor
            /// </summary>
            public Output() { }

            /// <summary>
            /// The MarketData Constructor by MarketDataEntity.Input
            /// </summary>
            public Output(MarketDataEntity.Input input)
            {
                if (input != null)
                {

                    this.MarketDataId = input.MarketDataId;
                    this.ETag = input.ETag;
                    this.ProviderName = input.ProviderName;
                    this.MarketDataName = input.MarketDataName;
                    this.OriginalGranularity = input.OriginalGranularity;
                    this.Type = input.Type;
                    this.OriginalTimezone = input.OriginalTimezone;
                    this.UnitOfMeasure = input.UnitOfMeasure;
                    this.AggregationRule = input.AggregationRule;
                    this.TransformID = input.TransformID;
                    this.ProviderDescription = input.ProviderDescription;
                    this.Tags = input.Tags;
                }
            }

            /// <summary>
            /// The TimeTransform
            /// </summary>
            [MessagePack.Key(11)]
            public TimeTransform? Transform { get; set; } //NULLABLE
            /// <summary>
            /// The Last time the metadata has been updated
            /// </summary>
            [MessagePack.Key(12)]
            public Instant LastUpdated { get; set; }
            /// <summary>
            /// The Last time the data has been written at
            /// </summary>
            [MessagePack.Key(13)]
            public Instant? DataLastWritedAt { get; set; }
            /// <summary>
            /// Start date of range for this curve  
            /// </summary>
            [MessagePack.Key(14)]
            public LocalDate? DataRangeStart { get; set; }
            /// <summary>
            /// End date of range for this curve  
            /// </summary>
            [MessagePack.Key(15)]
            public LocalDate? DataRangeEnd { get; set; }
            /// <summary>
            /// The time the market data has been created
            /// </summary>
            [MessagePack.Key(16)]
            public Instant Created { get; set; }
        }

        /// <summary>
        /// The Curve Ranges class
        /// </summary>
        public class WithRange : Output
        {
            /// <summary>
            /// The Curve Ranges
            /// </summary>
            public IEnumerable<CurveRange>? Curves { get; set; }
        }
    }
}
