using Artesian.SDK.Dto;
using Artesian.SDK.Dto.GMEPublicOffer;
using Flurl;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service.GMEPublicOffer
{
    /// <summary>
    /// GME Public Offer Query Class
    /// </summary>
    public class GMEPublicOfferQuery: IGMEPublicOfferQuery
    {
        private Client _client;
        private GMEPublicOfferQueryParamaters _queryParamaters = new GMEPublicOfferQueryParamaters();
        private static LocalDatePattern _localDatePattern = LocalDatePattern.Iso;

        internal GMEPublicOfferQuery(Client client)
        {
            _client = client;
        }

        /// <summary>
        /// Set the date to be queried
        /// </summary>
        /// <param name="date">Date to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForDate(LocalDate date)
        {
            _queryParamaters.Date = date;

            return this;
        }

        /// <summary>
        /// Set the Status to be queried
        /// </summary>
        /// <param name="status">Status to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForStatus(Status status)
        {
            _queryParamaters.Status = status;

            return this;
        }

        /// <summary>
        /// Set the Purpose to be queried
        /// </summary>
        /// <param name="purpose">Purpose to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForPurpose(Purpose purpose)
        {
            _queryParamaters.Purpose = purpose;

            return this;
        }

        /// <summary>
        /// Set the BATypes to be queried
        /// </summary>
        /// <param name="baType">BATypes to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForBAType(BAType[] baType)
        {
            _queryParamaters.BAType = baType;

            return this;
        }

        /// <summary>
        /// Set the generation types to be queried
        /// </summary>
        /// <param name="generationType">GenerationTypes to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForGenerationType(GenerationType[] generationType)
        {
            _queryParamaters.GenerationType = generationType;

            return this;
        }

        /// <summary>
        /// Set the markets to be queried
        /// </summary>
        /// <param name="market">Markets to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForMarket(Market[] market)
        {
            _queryParamaters.Market = market;

            return this;
        }

        /// <summary>
        /// Set the operators to be queried
        /// </summary>
        /// <param name="operator">Operators to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForOperator(string[] @operator)
        {
            _queryParamaters.Operator = @operator;

            return this;
        }

        /// <summary>
        /// Set the scopes to be queried
        /// </summary>
        /// <param name="scope">Scopes to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForScope(Scope[] scope)
        {
            _queryParamaters.Scope = scope;

            return this;
        }

        /// <summary>
        /// Set the units to be queried
        /// </summary>
        /// <param name="unit">Units to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForUnit(string[] unit)
        {
            _queryParamaters.Unit = unit;

            return this;
        }

        /// <summary>
        /// Set the unit types to be queried
        /// </summary>
        /// <param name="unitType">Unit types to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForUnitType(UnitType[] unitType)
        {
            _queryParamaters.UnitType = unitType;

            return this;
        }

        /// <summary>
        /// Set the zones to be queried
        /// </summary>
        /// <param name="zone">Zones to be queried</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery ForZone(Zone[] zone)
        {
            _queryParamaters.Zone = zone;

            return this;
        }

        /// <summary>
        /// Set the request pagination
        /// </summary>
        /// <param name="page">Result page number</param>
        /// <param name="pageSize">Result Page size</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery WithPagination(int page, int pageSize)
        {
            _queryParamaters.Page = page;
            _queryParamaters.PageSize = pageSize;

            return this;
        }

        /// <summary>
        /// Set the data sort
        /// </summary>
        /// <param name="sort">Sort by</param>
        /// <returns>GMEPublicOfferQuery</returns>
        public GMEPublicOfferQuery WithSort(string[] sort)
        {
            _queryParamaters.Sort = sort;

            return this;
        }

        /// <summary>
        /// Execute GME Public Offer Query
        /// </summary>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>Enumerable of TimeSerieRow Actual</returns>
        public async Task<PagedResult<GMEPublicOfferCurveDto>> ExecuteAsync(CancellationToken ctk = default)
        {
            string url = _buildRequest();

            var res = await _client.Exec<PagedResult<GMEPublicOfferCurveDto>>(HttpMethod.Get, url, ctk: ctk);
            return res;
        }


        #region private helpers
        private string _buildRequest()
        {
            _validateQuery();

            var url = $"/extract/{_localDatePattern.Format(_queryParamaters.Date.Value)}/{_queryParamaters.Purpose.ToString()}/{_queryParamaters.Status.ToString()}"
                        .SetQueryParam("operators", _queryParamaters.Operator)
                        .SetQueryParam("unit", _queryParamaters.Unit)
                        .SetQueryParam("market", _queryParamaters.Market)
                        .SetQueryParam("scope", _queryParamaters.Scope)
                        .SetQueryParam("baType", _queryParamaters.BAType)
                        .SetQueryParam("zone", _queryParamaters.Zone)
                        .SetQueryParam("unitType", _queryParamaters.UnitType)
                        .SetQueryParam("generationType", _queryParamaters.GenerationType)
                        .SetQueryParam("sort", _queryParamaters.Sort)
                        .SetQueryParam("page", _queryParamaters.Page)
                        .SetQueryParam("pageSize", _queryParamaters.PageSize)
                        .ToString();

            return url;
        }

        /// <summary>
        /// Validate query
        /// </summary>
        protected void _validateQuery()
        {
            if (_queryParamaters.Date == null)
                throw new ApplicationException("Date filter must be provided. Use .ForDate() to set date");
            
            if (_queryParamaters.Status == null)
                throw new ApplicationException("Status filter must be provided. Use .ForStatus() to set status");
            
            if (_queryParamaters.Purpose == null)
                throw new ApplicationException("Purpose filter must be provided. Use .ForPurpose() to set purpose");
        }
        #endregion
    }
}
