﻿using Artesian.SDK.Dto;
using NodaTime;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Factory
{
    /// <summary>
    /// Market Data Interface
    /// </summary>
    public interface IMarketData
    {
        /// <summary>
        /// MarketData Id
        /// </summary>
        int? MarketDataId { get; }

        /// <summary>
        /// MarketData Identifier
        /// </summary>
        MarketDataIdentifier Identifier { get; }

        /// <summary>
        /// MarketData ReadOnly Entity
        /// </summary>
        MarketDataMetadata Metadata { get; }

        /// <summary>
        /// MarketData Load Metadata
        /// </summary>
        /// <remarks>
        /// Update the MarketData
        /// </remarks>
        /// <returns></returns>
        Task Load(CancellationToken ctk = default);

        /// <summary>
        /// MarketData Update
        /// </summary>
        /// <remarks>
        /// Update the MarketData
        /// </remarks>
        /// <param name="ctk">Cancellation token</param>
        /// <returns></returns>
        Task Update(CancellationToken ctk = default);

        /// <summary>
        /// MarketData Register
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <param name="metadata">The entity of metadata</param>
        /// <param name="ctk">Cancellation token</param>
        /// <returns></returns>
        Task Register(MarketDataEntity.Input metadata, CancellationToken ctk = default);

        /// <summary>
        /// MarketData IsRegister
        /// </summary>
        /// <remarks>
        /// Register a MarketData
        /// </remarks>
        /// <returns> Marketdata if true, null and false if not found </returns>
        Task<bool> IsRegistered(CancellationToken ctk = default);

        /// <summary>
        /// Edit for Auction Timeserie
        /// </summary>
        /// <remarks>
        /// Start write mode for Auction Timeserie
        /// </remarks>
        /// <returns> Marketdata </returns>
        IAuctionMarketDataWritable EditAuction();

        /// <summary>
        /// Edit for Actual Timeserie
        /// </summary>
        /// <remarks>
        /// Start write mode for Actual Timeserie
        /// </remarks>
        /// <returns> Marketdata </returns>
        ITimeserieWritable EditActual();

        /// <summary>
        /// Edit for Versioned Timeserie
        /// </summary>
        /// <remarks>
        /// Start write mode for Versioned Timeserie
        /// </remarks>
        /// <returns> Marketdata </returns>
        ITimeserieWritable EditVersioned(LocalDateTime version);

        /// <summary>
        /// Edit for Market Assessment
        /// </summary>
        /// <remarks>
        /// Start write mode for Market Assessment
        /// </remarks>
        /// <returns> Marketdata </returns>
        IMarketAssessmentWritable EditMarketAssessment();

        /// <summary>
        /// Edit for Bid Ask
        /// </summary>
        /// <remarks>
        /// Start write mode for Bid Ask
        /// </remarks>
        /// <returns> Marketdata </returns>
        IBidAskWritable EditBidAsk();
    }

    /// <summary>
    /// Interface for Market Assessment Write
    /// </summary>
    public interface IMarketAssessmentWritable
    {
        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// MarketAssessment AddData
        /// </remarks>
        /// <param name="localDate">The local date of the value</param>
        /// <param name="product">The product</param>
        /// <param name="value">Market assessment Value</param>
        /// <returns></returns>
        AddAssessmentOperationResult AddData(LocalDate localDate, string product, MarketAssessmentValue value);

        /// <summary>
        /// MarketAssessment AddData
        /// </summary>
        /// <remarks>
        /// MarketAssessment AddData
        /// </remarks>
        /// <param name="time">The instant of the value</param>
        /// <param name="product">The product</param>
        /// <param name="value">Market assessment Value</param>
        /// <returns></returns>
        AddAssessmentOperationResult AddData(Instant time, string product, MarketAssessmentValue value);

        /// <summary>
        /// MarketAssessment ClearData
        /// </summary>
        void ClearData();

        /// <summary>
        /// MarketAssessment Save
        /// </summary>
        /// <remarks>
        /// MarketAssessment Save
        /// </remarks>
        /// <param name="downloadedAt">The instant downloaded</param>
        /// <param name="deferCommandExecution">Defer Command Execution</param>
        /// <param name="deferDataGeneration">Defer Data Generation</param>
        /// <param name="keepNulls">if <see langword="false"/> Settlement=null are ignored (server-side). That is the default behaviour.</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default);

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="product">Product of the MarketAssessment Time Serie</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, List<string> product = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default);
    }

    /// <summary>
    /// Interface for Bid Ask Write
    /// </summary>
    public interface IBidAskWritable
    {
        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// BidAsk AddData
        /// </remarks>
        /// <param name="localDate">The local date of the value</param>
        /// <param name="product">The product</param>
        /// <param name="value">Bid Ask Value</param>
        /// <returns></returns>
        AddBidAskOperationResult AddData(LocalDate localDate, string product, BidAskValue value);

        /// <summary>
        /// BidAsk AddData
        /// </summary>
        /// <remarks>
        /// BidAsk AddData
        /// </remarks>
        /// <param name="time">The instant of the value</param>
        /// <param name="product">The product</param>
        /// <param name="value">Bid Ask Value</param>
        /// <returns></returns>
        AddBidAskOperationResult AddData(Instant time, string product, BidAskValue value);

        /// <summary>
        /// BidAsk ClearData
        /// </summary>
        void ClearData();

        /// <summary>
        /// BidAsk Save
        /// </summary>
        /// <remarks>
        /// BidAsk Save
        /// </remarks>
        /// <param name="downloadedAt">The instant downloaded</param>
        /// <param name="deferCommandExecution">Defer Command Execution</param>
        /// <param name="deferDataGeneration">Defer Data Generation</param>
        /// <param name="keepNulls">if <see langword="false"/></param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default);

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="product">Product of the BidAsk Time Serie</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, List<string> product = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default);
    }
    /// <summary>
    /// Interface for Auction Bid Write
    /// </summary>
    public interface IAuctionMarketDataWritable
    {
        /// <summary>
        /// Auction AddData
        /// </summary>
        /// <remarks>
        /// Auction AddData
        /// </remarks>
        /// <param name="localDate">The local date of the value</param>
        /// <param name="bid">The bid</param>
        /// <param name="offer">The offer</param>
        /// <returns></returns>
        AddAuctionTimeSerieOperationResult AddData(LocalDate localDate, AuctionBidValue[] bid, AuctionBidValue[] offer);

        /// <summary>
        /// Auction AddData
        /// </summary>
        /// <remarks>
        /// Auction AddData
        /// </remarks>
        /// <param name="time">The instant of the value</param>
        /// <param name="bid">The bid</param>
        /// <param name="offer">The offer</param>
        /// <returns></returns>
        AddAuctionTimeSerieOperationResult AddData(Instant time, AuctionBidValue[] bid, AuctionBidValue[] offer);

        /// <summary>
        /// Auction ClearData
        /// </summary>
        void ClearData();

        /// <summary>
        /// Auction Save
        /// </summary>
        /// <remarks>
        /// Auction Save
        /// </remarks>
        /// <param name="downloadedAt">The instant downloaded</param>
        /// <param name="deferCommandExecution">Defer Command Execution</param>
        /// <param name="deferDataGeneration">Defer Data Generation</param>
        /// <param name="keepNulls">if <see langword="false"/> nulls are ignored (server-side). That is the default behaviour.</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default);

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default);    }

    /// <summary>
    /// Interface for timeserie write
    /// </summary>
    /// <remarks>
    /// Common for Actual and Versioned timeserie
    /// </remarks>
    /// <returns> Marketdata </returns>
    public interface ITimeserieWritable
    {
        /// <summary>
        /// TimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with localDate
        /// </remarks>
        /// <param name="localDate">The local date of the value</param>
        /// <param name="value">Value</param>
        /// <returns>AddTimeSerieOperationResult</returns>
        AddTimeSerieOperationResult AddData(LocalDate localDate, double? value);

        /// <summary>
        /// TimeSerie AddData
        /// </summary>
        /// <remarks>
        /// Add Data on to the curve with Instant
        /// </remarks>
        /// <param name="time">The instant of the value</param>
        /// <param name="value">Value</param>
        /// <returns>AddTimeSerieOperationResult</returns>
        AddTimeSerieOperationResult AddData(Instant time, double? value);

        /// <summary>
        /// TimeSerie ClearData
        /// </summary>
        /// <remarks>
        /// Clear all the data set in the Values
        /// </remarks>
        /// <returns></returns>
        void ClearData();

        /// <summary>
        /// TimeSerie Save
        /// </summary>
        /// <remarks>
        /// TimeSerie Save
        /// </remarks>
        /// <param name="downloadedAt">The instant downloaded</param>
        /// <param name="deferCommandExecution">Defer Command Execution</param>
        /// <param name="deferDataGeneration">Defer Data Generation</param>
        /// <param name="keepNulls">if <see langword="false"/> nulls are ignored (server-side). That is the default behaviour.</param>
        /// <param name="ctk">Cancellation Token</param>
        /// <returns></returns>
        Task Save(Instant downloadedAt, bool deferCommandExecution = false, bool deferDataGeneration = true, bool keepNulls = false, CancellationToken ctk = default);

        /// <summary>
        /// MarketData Delete
        /// </summary>
        /// <remarks>
        /// Delete the Data of the current MarketData
        /// </remarks>
        /// <param name="rangeStart">LocalDateTime start of range to be deleted (in case of null, LocalDateTime MinIso value will be used)</param>
        /// <param name="rangeEnd">LocalDateTime end of range to be deleted (in case of null, LocalDateTime MaxIso value will be used)</param>
        /// <param name="deferCommandExecution">DeferCommandExecution</param>
        /// <param name="deferDataGeneration">DeferDataGeneration</param>
        /// <param name="ctk">The Cancellation Token</param> 
        /// <returns></returns>
        Task Delete(LocalDateTime? rangeStart = null, LocalDateTime? rangeEnd = null, bool deferCommandExecution = false, bool deferDataGeneration = true, CancellationToken ctk = default);
    }
}