using Artesian.SDK.Dto.DerivedCfg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.SDK.Dto.MarketData
{
    internal static class MarketDataEntityInputExt
    {
        public static void ValidateRegister(this MarketDataEntity.Input marketDataEntityInput)
        {
            if (marketDataEntityInput.MarketDataId != 0)
                throw new ArgumentException("MarketDataId must be 0", nameof(marketDataEntityInput));

            if (marketDataEntityInput.Type == MarketDataType.MarketAssessment && marketDataEntityInput.TransformID != null)
                throw new ArgumentException("No transform possible when Type is MarketAssessment", nameof(marketDataEntityInput));
        }

        public static void ValidateUpdate(this MarketDataEntity.Input marketDataEntityInput)
        {
            if (marketDataEntityInput.Type == MarketDataType.MarketAssessment && marketDataEntityInput.TransformID != null)
                throw new ArgumentException("No transform possible when Type is MarketAssessment", nameof(marketDataEntityInput));

            if (marketDataEntityInput.Type == MarketDataType.MarketAssessment && marketDataEntityInput.AggregationRule != AggregationRule.Undefined)
                throw new ArgumentException("Aggregation Rule must be Undefined if Type is MarketAssessment", nameof(marketDataEntityInput));
        }

        public static void ValidateDerivedCfg(this MarketDataEntity.Output marketDataEntityOutput, DerivedCfgBase derivedCfg)
        {
            if (marketDataEntityOutput.DerivedCfg == null)
                throw new ArgumentException("Derived Configuration is null for the MarketData selected", nameof(marketDataEntityOutput));

            if (marketDataEntityOutput.DerivedCfg.DerivedAlgorithm != derivedCfg.DerivedAlgorithm)
                throw new ArgumentException("Derived Algorithm is different from the one in the update", nameof(marketDataEntityOutput));
        }
    }
}
