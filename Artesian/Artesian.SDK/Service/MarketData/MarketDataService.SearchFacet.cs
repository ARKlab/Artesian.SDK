// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Artesian.SDK.Dto;

using Flurl;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {

        /// <summary>
        /// Search the marketdata metadata
        /// </summary>
        /// <param name="filter">ArtesianSearchFilter containing the search params</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ArtesianSearchResults entity</returns>
        public Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, CancellationToken ctk = default)
            => SearchFacetAsync(filter: filter, doNotLoadAdditionalInfo: false, includeCurveSummary: false, includeTimeTransform: false, includeDataQuality: false, skipOverrides: true, ctk: ctk);

        /// <summary>
        /// Search the marketdata metadata
        /// </summary>
        /// <param name="filter">ArtesianSearchFilter containing the search params</param>
        /// <param name="doNotLoadAdditionalInfo">Skip loading up-to-date curve range and transform</param>
        /// <param name="includeCurveSummary">When true, includes curve summary (ranges) in the response</param>
        /// <param name="includeTimeTransform">When true, includes time transform in the response</param>
        /// <param name="includeDataQuality">When true, includes data quality status summary in the response</param>
        /// <param name="skipOverrides">When false, composes original and override metadata. Requires the /featureflag/overridebeta permission during beta. Default true.</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>ArtesianSearchResults entity</returns>
        public Task<ArtesianSearchResults> SearchFacetAsync(ArtesianSearchFilter filter, bool doNotLoadAdditionalInfo = false, bool includeCurveSummary = false, bool includeTimeTransform = false, bool includeDataQuality = false, bool skipOverrides = true, CancellationToken ctk = default)
        {
            filter.Validate();

            var url = "/marketdata/searchfacet"
                    .SetQueryParam("pageSize", filter.PageSize)
                    .SetQueryParam("page", filter.Page)
                    .SetQueryParam("searchText", filter.SearchText)
                    .SetQueryParam("filters", filter.Filters?.SelectMany(s => s.Value.Select(x => $@"{s.Key}:{x}")))
                    .SetQueryParam("sorts", filter.Sorts)
                    .SetQueryParam("doNotLoadAdditionalInfo", doNotLoadAdditionalInfo)
                    .SetQueryParam("includeCurveSummary", includeCurveSummary)
                    .SetQueryParam("includeTimeTransform", includeTimeTransform)
                    .SetQueryParam("includeDataQuality", includeDataQuality)
                    .SetQueryParam("skipOverrides", skipOverrides);

            return _client.Exec<ArtesianSearchResults>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
