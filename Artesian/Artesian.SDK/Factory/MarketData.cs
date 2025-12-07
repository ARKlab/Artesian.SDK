using Artesian.SDK.Common;
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.MarketData;
using Artesian.SDK.Service;

using NodaTime;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Market Data Generic class
    /// </summary>
    public sealed class MarketData : IMarketData
    {
        internal IMarketDataService _marketDataService;
        internal MarketDataEntity.Output? _entity = null;

        /// <summary>
        /// MarketData Id
        /// </summary>
        public int? MarketDataId { get { return _entity?.MarketDataId; } }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        public MarketDataIdentifier Identifier { get; private set; }

        /// <summary>
        /// MarketData Entity
        /// </summary>
        public MarketDataMetadata? Metadata { get; private set; }

        /// <summary>
        /// DerivedCfg
        /// </summary>
        public DerivedCfg? DerivedCfg { get; private set; }

        /// <summary>
        /// MarketData Constructor by Id
        /// </summary>
        internal MarketData(IMarketDataService marketDataService, MarketDataIdentifier id)
        {
            Guard.IsNotNull(marketDataService, nameof(marketDataService));

            _marketDataService = marketDataService;

            Identifier = id;
        }

        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">The entity of metadata</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns></returns>
        public async Task Register(MarketDataEntity.Input metadata, CancellationToken ctk = default)
        {
            Guard.IsNotNull(metadata, nameof(metadata));
            Guard.IsTrue(metadata.ProviderName == null || string.Equals(metadata.ProviderName, this.Identifier.Provider));
            Guard.IsTrue(metadata.ProviderName == null || string.Equals(metadata.ProviderName, this.Identifier.Provider));
            Guard.IsTrue(metadata.MarketDataName == null || string.Equals(metadata.MarketDataName, this.Identifier.Name));
            Guard.IsNotNullOrWhiteSpace(metadata.OriginalTimezone);

            metadata.ProviderName = this.Identifier.Provider;
            metadata.MarketDataName = this.Identifier.Name;

            if (_entity != null)
                throw new ArtesianFactoryException("Market Data is already registered with ID {0}", _entity.MarketDataId);

            _entity = await _marketDataService.RegisterMarketDataAsync(metadata, ctk).ConfigureAwait(false);

            DerivedCfg = new DerivedCfg(_entity.DerivedCfg);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> True if  Marketdata is present, false if not found </returns>
        public async Task<bool> IsRegistered(CancellationToken ctk = default)
        {
            if (Metadata != null)
                return true;

            await Load(ctk).ConfigureAwait(false);

            if (Metadata != null)
                return true;

            return false;
        }

        /// <summary>
        /// Loads MarketData Metadata
        /// </summary>
        /// <remarks>
        /// Loads MarketData Metadata
        /// </remarks>
        /// <returns></returns>
        public async Task Load(CancellationToken ctk = default)
        {
            _entity = await _marketDataService.ReadMarketDataRegistryAsync(Identifier, ctk).ConfigureAwait(false);

            if (_entity != null)
            {
                DerivedCfg = new DerivedCfg(_entity.DerivedCfg);

                Metadata = new MarketDataMetadata(_entity);
            }
        }

        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData
        /// </remarks>
        /// <returns></returns>
        public async Task Update(CancellationToken ctk = default)
        {
            if (_entity == null)
                throw new ArtesianFactoryException("Market Data is not yet registered");

            var marketDataEntityInput = new MarketDataEntity.Input(_entity);

            _entity = await _marketDataService.UpdateMarketDataAsync(marketDataEntityInput, ctk).ConfigureAwait(false);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// Actual Timeserie Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Actual Timeserie
        /// </remarks>
        /// <returns> ITimeserieWritable </returns>
        public ITimeserieWritable EditActual()
        {
            if (_entity == null)
                throw new ActualTimeSerieException("Actual Time Serie is not yet registered");

            if (_entity.Type != MarketDataType.ActualTimeSerie)
                throw new MarketAssessmentException("Entity is not an Actual Time Serie");

            var actual = new ActualTimeSerie(this);
            return actual;
        }

        /// <summary>
        /// Auction Timeserie Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Auction Timeserie
        /// </remarks>
        /// <returns> ITimeserieWritable </returns>
        public IAuctionMarketDataWritable EditAuction()
        {
            if (_entity == null)
                throw new AuctionTimeSerieException("Auction Time Serie is not yet registered");

            if (_entity.Type != MarketDataType.Auction)
                throw new MarketAssessmentException("Entity is not an Auction Time Serie");

            var auction = new AuctionTimeSerie(this);
            return auction;
        }

        /// <summary>
        /// Versioned Timeserie Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Versioned Timeserie
        /// </remarks>
        /// <returns> ITimeserieWritable </returns>
        public ITimeserieWritable EditVersioned(LocalDateTime version)
        {
            if (_entity == null)
                throw new VersionedTimeSerieException("Versioned Time Serie is not yet registered");

            if (_entity.Type != MarketDataType.VersionedTimeSerie)
                throw new MarketAssessmentException("Entity is not  a Versioned Time Serie");

            var versioned = new VersionedTimeSerie(this);
            versioned.SetSelectedVersion(version);

            return versioned;
        }

        /// <summary>
        /// Market Assessment Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Market Assessment
        /// </remarks>
        /// <returns> IMarketAssessmentWritable </returns>
        public IMarketAssessmentWritable EditMarketAssessment()
        {
            if (_entity == null)
                throw new MarketAssessmentException("Market Assessement is not yet registered");

            if (_entity.Type != MarketDataType.MarketAssessment)
                throw new MarketAssessmentException("Entity is not a Market Assessement");

            var mas = new MarketAssessment(this);
            return mas;
        }

        /// <summary>
        /// Bid Ask Edit
        /// </summary>
        /// <remarks>
        /// Start write mode for Bid Ask
        /// </remarks>
        /// <returns> IBidAskWritable </returns>
        public IBidAskWritable EditBidAsk()
        {
            if (_entity == null)
                throw new BidAskException("Bid Ask is not yet registered");

            if (_entity.Type != MarketDataType.BidAsk)
                throw new BidAskException("Entity is not a Bid Ask");

            var bask = new BidAsk(this);
            return bask;
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Muv to be updated</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgMuv derivedCfg, CancellationToken ctk = default)
        {
            await UpdateDerivedConfiguration(derivedCfg, false, ctk).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Muv to be updated</param>
        /// <param name="force">Force the update of configuration also if another rebuild process is running (Defualt=false)</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgMuv derivedCfg, bool force, CancellationToken ctk = default)
        {
            _entity!.ValidateUpdateDerivedCfg(derivedCfg);

            _entity = await _marketDataService.UpdateDerivedConfigurationAsync(_entity.MarketDataId, derivedCfg, force, ctk).ConfigureAwait(false) ?? throw new InvalidOperationException("UpdateDerivedConfigurationAsync returned null");

            DerivedCfg = new DerivedCfg(_entity.DerivedCfg);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Coalesce to be updated</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgCoalesce derivedCfg, CancellationToken ctk = default)
        {
            await UpdateDerivedConfiguration(derivedCfg, false, ctk).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Coalesce to be updated</param>
        /// <param name="force">Force the update of configuration also if another rebuild process is running (Defualt=false)</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgCoalesce derivedCfg, bool force, CancellationToken ctk = default)
        {
            _entity!.ValidateUpdateDerivedCfg(derivedCfg);

            _entity = await _marketDataService.UpdateDerivedConfigurationAsync(_entity.MarketDataId, derivedCfg, force, ctk).ConfigureAwait(false);

            DerivedCfg = new DerivedCfg(_entity.DerivedCfg);

            Metadata = new MarketDataMetadata(_entity);
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Sum to be updated</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgSum derivedCfg, CancellationToken ctk = default)
        {
            await UpdateDerivedConfiguration(derivedCfg, false, ctk).ConfigureAwait(false);
        }

        /// <summary>
        /// Update Derived Configuration
        /// </summary>
        /// <param name="derivedCfg">The Derived Configuration Sum to be updated</param>
        /// <param name="force">Force the update of configuration also if another rebuild process is running (Defualt=false)</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        public async Task UpdateDerivedConfiguration(DerivedCfgSum derivedCfg, bool force, CancellationToken ctk = default)
        {
            _entity!.ValidateUpdateDerivedCfg(derivedCfg);

            _entity = await _marketDataService.UpdateDerivedConfigurationAsync(_entity.MarketDataId, derivedCfg, force, ctk).ConfigureAwait(false);

            DerivedCfg = new DerivedCfg(_entity.DerivedCfg);

            Metadata = new MarketDataMetadata(_entity);
        }
    }
}