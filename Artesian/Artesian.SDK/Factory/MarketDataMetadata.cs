using Artesian.SDK.Dto;
using Artesian.SDK.Dto.UoM;

using NodaTime;

using System.Collections.Generic;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Read Only Class for MarketData Entity
    /// </summary>
    public sealed class MarketDataMetadata
    {
        private readonly MarketDataEntity.Output _output;

        /// <summary>
        /// Read Only Class for MarketData Entity constructor
        /// </summary>
        internal MarketDataMetadata(MarketDataEntity.Output output)
        {
            _output = output;
        }

        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        public int MarketDataId => _output.MarketDataId;
        /// <summary>
        /// The Market Data Etag
        /// </summary>
        public string ETag => _output.ETag;
        /// <summary>
        /// The Market Data Provider Name
        /// </summary>
        public string ProviderName => _output.ProviderName;
        /// <summary>
        /// The Market Data Name
        /// </summary>
        public string MarketDataName => _output.MarketDataName;
        /// <summary>
        /// The Original Granularity
        /// </summary>
        public Granularity OriginalGranularity => _output.OriginalGranularity;
        /// <summary>
        /// The Unit of Measure
        /// </summary>
        public UnitOfMeasure? UnitOfMeasure { get => _output.UnitOfMeasure; set => _output.UnitOfMeasure = value; }
        /// <summary>
        /// The Type
        /// </summary>
        public MarketDataType Type => _output.Type;
        /// <summary>
        /// The Original Timezone
        /// </summary>
        public string OriginalTimezone { get => _output.OriginalTimezone; set => _output.OriginalTimezone = value; }
        /// <summary>
        /// The Aggregation Rule
        /// </summary>
        public AggregationRule AggregationRule { get => _output.AggregationRule; set => _output.AggregationRule = value; }
        /// <summary>
        /// The Provider description
        /// </summary>
        public string ProviderDescription { get => _output.ProviderDescription; set => _output.ProviderDescription = value; }
        /// <summary>
        /// The custom Tags assigned to the data
        /// </summary>
        public IDictionary<string, List<string>>? Tags { get => _output.Tags; set => _output.Tags = value; }
        /// <summary>
        /// The Authorization Path
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The TimeTransform
        /// </summary>
        public TimeTransform? Transform { get => _output.Transform; set { _output.Transform = value; _output.TransformID = _output.Transform?.ID; } }
        /// <summary>
        /// The Last time the metadata has been updated
        /// </summary>
        public Instant LastUpdated => _output.LastUpdated;
        /// <summary>
        /// The Last time the data has been written
        /// </summary>
        public Instant? DataLastWritedAt => _output.DataLastWritedAt;
        /// <summary>
        /// Start date of range for this curve  
        /// </summary>
        public LocalDate? DataRangeStart => _output.DataRangeStart;
        /// <summary>
        /// End date of range for this curve  
        /// </summary>
        public LocalDate? DataRangeEnd => _output.DataRangeEnd;
        /// <summary>
        /// The time the market data has been created
        /// </summary>
        public Instant Created => _output.Created;
    }
}
