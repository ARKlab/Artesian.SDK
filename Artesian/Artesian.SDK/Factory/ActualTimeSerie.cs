﻿using Artesian.SDK.Dto;
using Artesian.SDK.Common;
using Artesian.SDK.Service;
using EnsureThat;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// ActualTimeSerie entity
    /// </summary>
    public class ActualTimeSerie : IMarketData
    {
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        //Class members
        private readonly IMetadataService _metadataService;
        private MarketDataEntity.Output _entity = null;
        private MarketDataEntity.Input _metadata = null;

        private bool _isInWriteMode = false;

        private Dictionary<LocalDateTime, double?> _values = new Dictionary<LocalDateTime, double?>();

        /// <summary>
        /// ActualTimeSerie Curve Values
        /// </summary>
        public IReadOnlyDictionary<LocalDateTime, double?> Values { get; private set; }

        /// <summary>
        /// MarketData Id
        /// </summary>
        public int MarketDataId { get { return _entity == null ? 0 : _entity.MarketDataId; } }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        public MarketDataIdentifier Identifier { get; protected set; }

        /// <summary>
        /// MarketData DataTimezone
        /// </summary>
        public string DataTimezone
        {
            get
            {
                if (_entity?.OriginalGranularity.IsTimeGranularity() == true)
                    return "UTC";
                else
                    return _entity?.OriginalTimezone;
            }
        }

        /// <summary>
        /// MarketData Type
        /// </summary>
        public MarketDataType Type => MarketDataType.ActualTimeSerie;

        /// <summary>
        /// MarketData Granularity
        /// </summary>
        public Granularity Granularity { get { return _entity == null ? default : _entity.OriginalGranularity; } }

        /// <summary>
        /// MarketData Timezone
        /// </summary>
        public string Timezone => _entity?.OriginalTimezone;

        /// <summary>
        /// MarketData Tags
        /// </summary>
        public Dictionary<string, List<string>> Tags { get { return _entity?.Tags; } }

        /// <summary>
        /// ActualTimeSerie Constructor
        /// </summary>
        public ActualTimeSerie(IMetadataService metadataService, MarketDataEntity.Output entity)
        {
            EnsureArg.IsNotNull(metadataService, nameof(metadataService));

            _metadataService = metadataService;
            _create(entity);

            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }

        private void _create(MarketDataEntity.Output entity)
        {
            Identifier = new MarketDataIdentifier(entity.ProviderName, entity.MarketDataName);
            _entity = entity;
        }

        #region Interface Methods
        /// <summary>
        /// MarketData ClearData
        /// </summary>
        public void ClearData()
        {
            _values = new Dictionary<LocalDateTime, double?>();
            Values = new ReadOnlyDictionary<LocalDateTime, double?>(_values);
        }
        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">the entity of metadata</param>
        /// <returns></returns>
        public async Task Register(MarketDataEntity.Input metadata)
        {
            EnsureArg.IsNotNull(metadata, nameof(metadata));
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.ProviderName == null || metadata.ProviderName == this.Identifier.Provider);
            EnsureArg.IsTrue(metadata.MarketDataName == null || metadata.MarketDataName == this.Identifier.Name);
            EnsureArg.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new ActualTimeSerieException("Actual Time Serie is already registered with ID {0}", _entity.MarketDataId);

            _entity = await _metadataService.RegisterMarketDataAsync(metadata);
        }
        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> Marketdata if true, null and false if not found </returns>
        public async Task<(MarketDataEntity.Output, bool)> IsRegistered()
        {
           if (_entity == null)
               _entity = await _metadataService.ReadMarketDataRegistryAsync(this.Identifier);

            if (_entity != null)
                return (_entity, true);

            return (null, false);
        }
        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        public MarketDataEntity.Input LoadMetadata()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            _metadata = _entity;

            return _metadata;
        }
        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData 
        /// </remarks>
        /// <returns></returns>
        public async Task Update(MarketDataEntity.Input metadata)
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            _entity = await _metadataService.UpdateMarketDataAsync(metadata);
        }
        #endregion

        #region Write
        /// <summary>
        /// ActualTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with localDate
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(LocalDate localDate, double? value)
        {
            Ensure.Any.IsNotNull(_entity);
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Time granularity. Use AddData(Instant time, double? value)");

            var localTime = localDate.AtMidnight();

            return _add(localTime, value);
        }
        /// <summary>
        /// ActualTimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on curve with Instant
        /// </remarks>
        /// <returns>AddTimeSerieOperationResult</returns>
        public AddTimeSerieOperationResult AddData(Instant time, double? value)
        {
            Ensure.Any.IsNotNull(_entity);
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (!_entity.OriginalGranularity.IsTimeGranularity())
                throw new ActualTimeSerieException("This MarketData has Date granularity. Use AddData(LocalDate date, double? value)");

            var localTime = time.InUtc().LocalDateTime;

            return _add(localTime, value);
        }

        private AddTimeSerieOperationResult _add(LocalDateTime localTime, double? value)
        {
            if (_values.ContainsKey(localTime))
                return AddTimeSerieOperationResult.TimeAlreadyPresent;

            if (_entity.OriginalGranularity.IsTimeGranularity())
            {
                var period = ArtesianUtils.MapTimePeriod(_entity.OriginalGranularity);
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }
            else
            {
                var period = ArtesianUtils.MapDatePeriod(_entity.OriginalGranularity);
                if (!localTime.IsStartOfInterval(period))
                    throw new ArtesianSdkClientException("Trying to insert Time {0} with wrong format to serie {1}. Should be of period {2}", localTime, Identifier, period);
            }

            _values.Add(localTime, value);
            return AddTimeSerieOperationResult.ValueAdded;
        }

        /// <summary>
        /// ActualTimeSerie Edit
        /// </summary>
        /// <remarks>
        /// Returns the ActualTimeSerie to start write operations
        /// </remarks>
        /// <returns>ActualTimeSerie</returns>
        public ActualTimeSerie EditActual()
        {
            Ensure.Any.IsNotNull(_entity);
            _isInWriteMode = true;

            return this;
        }
        /// <summary>
        /// MarketData Save
        /// </summary>
        /// <remarks>
        /// Save the Data of the current MarketData
        /// </remarks>
        /// <param name="downloadedAt">downloaded at</param>
        /// <param name="deferCommandExecution">deferCommandExecution</param>
        /// <param name="deferDataGeneration">deferDataGeneration</param>
        /// <returns></returns>
        public async Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true)
        {
            Ensure.Any.IsNotNull(_entity);
            Ensure.Bool.IsTrue(_isInWriteMode);

            if (_values.Any())
            {
                var data = new UpsertCurveData(this.Identifier)
                {
                    Timezone = _entity.OriginalGranularity.IsTimeGranularity() ? "UTC" : _entity.OriginalTimezone,
                    DownloadedAt = downloadedAt,
                    Rows = _values,
                    DeferCommandExecution = deferCommandExecution,
                    DeferDataGeneration = deferDataGeneration
                };

                await _metadataService.UpsertCurveDataAsync(data);
            }
            //else
            //    _logger.Warn("No Data to be saved.");
        }
        #endregion
    }
}
