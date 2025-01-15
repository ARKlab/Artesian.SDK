using System;

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

        public static void ValidateUpdateDerivedCfg(this MarketDataEntity.Output marketDataEntityOutput, DerivedCfgBase derivedCfg)
        {
            if (marketDataEntityOutput.DerivedCfg == null)
                throw new ArgumentException("DerivedCfg cannot be added to a MarketData that has not", nameof(marketDataEntityOutput));

            if (marketDataEntityOutput.DerivedCfg.DerivedAlgorithm != derivedCfg.DerivedAlgorithm)
                throw new ArgumentException("Derived Algorithm cannot be update", nameof(marketDataEntityOutput));
        }
    }
}
