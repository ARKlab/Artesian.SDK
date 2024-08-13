using Artesian.SDK.Dto;
using Artesian.SDK.Service;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// MetadataService Extension
    /// </summary>
    public static class MarketDataServiceExtension
    {
        /// <summary>
        /// Read marketdata entity by id and returns an istance of IMarketData if it exists
        /// </summary>
        /// <param name="marketDataService">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="id">MarketDataIdentifier of markedata to be retrieved</param>
        /// <returns>IMarketData</returns>
        public static IMarketData GetMarketDataReference(this IMarketDataService marketDataService, MarketDataIdentifier id)
        {
            return new MarketData(marketDataService, id);
        }

        /// <summary>
        /// Read marketdata entity by MarketDataIdentifier and returns an istance of IMarketData if exists
        /// </summary>
        /// <param name="marketDataService">MarketDataIdentifier of markedata to be retrieved</param>
        /// <param name="provider">MarketDataIdentifier provider</param>
        /// <param name="name">MarketDataIdentifier name</param>
        /// <returns>IMarketData</returns>
        public static IMarketData GetMarketDataReference(this IMarketDataService marketDataService, string provider, string name)
        {
            return new MarketData(marketDataService, new MarketDataIdentifier(provider, name));
        }
    }
}
