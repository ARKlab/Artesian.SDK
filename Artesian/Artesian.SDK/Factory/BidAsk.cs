using Ark.Tools.Nodatime;

using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// BidAsk entity
    /// </summary>
    internal sealed class BidAsk : IBidAskWritable
    {
        private IMarketDataService _marketDataService;
        private MarketDataEntity.Output _entity = null;
        private readonly MarketDataIdentifier _identifier = null;

        /// <summary>
        /// BidAsks Constructor
        /// </summary>
        internal BidAsk(MarketData marketData)
        {
            _entity = marketData._entity;
            _marketDataService = marketData._marketDataService;

            _identifier = new MarketDataIdentifier(_entity.ProviderName, _entity.MarketDataName);

            BidAsks = new List<BidAskElement>();
        }

        /// <summary>
        /// BidAsk BidAskElement
        /// </summary>
        public List<BidAskElement> BidAsks { get; internal set; }

        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            BidAsks.Clear();
        }

        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <returns>AddBidAskOperationResult</returns>
        public AddBidAskOperationResult AddData(LocalDate localDate, string product, BidAskValue value)
        {
            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Time granularity. Use AddData(Instant time...)");

            return _addBidAsk(localDate.AtMidnight(), product, value);
        }
        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <returns>AddBidAskOperationResult</returns>
        public AddBidAskOperationResult AddData(Instant time, string product, BidAskValue value)
        {
            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new BidAskException("This MarketData has Date granularity. Use AddData(LocalDate date...)");

            return _addBidAsk(time.InUtc().LocalDateTime, product, value);
        }

        private AddBidAskOperationResult _addBidAsk(LocalDateTime reportTime, string product, BidAskValue value)
        {
            //Relative products
            if (Regex.IsMatch(product, @"\+\d+$"))
                throw new NotSupportedException("Relative Products are not supported");

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new BidAskException("Trying to insert Report Time {0} with the wrong format to BidAsk {1}. Should be of period {2}", reportTime, _identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!reportTime.IsStartOfInterval(period))
                    throw new BidAskException("Trying to insert Report Time {0} with wrong the format to BidAsk {1}. Should be of period {2}", reportTime, _identifier, period);
            }


            if (BidAsks.Any(row => row.ReportTime == reportTime && row.Product.Equals(product)))
                return AddBidAskOperationResult.ProductAlreadyPresent;

            BidAsks.Add(new BidAskElement(reportTime, product, value));
            return AddBidAskOperationResult.BidAskAdded;
        }

        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">Downloaded at</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="keepNulls">if <see langword="false"/> nulls are ignored (server-side). That is the default behaviour.</param>
        /// <param name="ctk">The Cancellation Token</param>
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default)
        {
            Ensure.Any.IsNotNull(_entity);

            if (BidAsks.Any())
            {
                var data = new UpsertCurveData(_identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    DeferCommandExecution = deferCommandExecution,
                    BidAsk = new Dictionary<LocalDateTime, IDictionary<string, BidAskValue>>(),
                    KeepNulls = keepNulls
                };

                foreach (var reportTime in BidAsks.GroupBy(g => g.ReportTime))
                {
                    var BidAsks = reportTime.ToDictionary(key => key.Product.ToString(), value => value.Value);
                    data.BidAsk.Add(reportTime.Key, BidAsks);
                }

                await _marketDataService.UpsertCurveDataAsync(data, ctk);
            }
        }


        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="product">Product of the BidAsk Time Serie</param>
        /// <param name="timezone">Timezone of the delete range. Default is CET</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        public async Task Delete(LocalDateTime? rangeStart, LocalDateTime? rangeEnd, List<string> product = null, string timezone = "CET", bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default)
        {
            Ensure.Any.IsNotNull(_entity);

            var timeZone = DateTimeZoneProviders.Tzdb[timezone];

            var data = new DeleteCurveData(_identifier)
            {
                Timezone = timezone,
                // LocalDate.MinIsoValue has year -9998 and yearOfEra 9999. Using it without any string formatting, we got date 01-01-9999.
                // So we use default(LocalDateTime) 01/01/0001
                RangeStart = rangeStart ?? default(LocalDateTime),
                RangeEnd = rangeEnd ?? LocalDateTime.MaxIsoValue.Date.AtStartOfDayInZone(timeZone).LocalDateTime,
                DeferCommandExecution = deferCommandExecution,
                DeferDataGeneration = deferDataGeneration,
                Product = product,
            };

            await _marketDataService.DeleteCurveDataAsync(data, ctk);
        }

        /// <summary>
        /// BidAskElement entity
        /// </summary>
        public class BidAskElement
        {
            /// <summary>
            /// BidAskElement constructor
            /// </summary>
            public BidAskElement(LocalDateTime reportTime, string product, BidAskValue value)
            {
                ReportTime = reportTime;
                Product = product;
                Value = value;
            }

            /// <summary>
            /// BidAskElement ReportTime
            /// </summary>
            public LocalDateTime ReportTime { get; set; }
            /// <summary>
            /// BidAskElement Product
            /// </summary>
            public string Product { get; set; }
            /// <summary>
            /// BidAskElement BidAskValue
            /// </summary>
            public BidAskValue Value { get; set; }
        }
    }
}
