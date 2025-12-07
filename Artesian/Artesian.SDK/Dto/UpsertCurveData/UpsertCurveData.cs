using MessagePack;
using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The curve data for a save command.
    /// </summary>
    [MessagePackObject]
    public class UpsertCurveData
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public UpsertCurveData()
        {
        }

        /// <summary>
        /// The constructor with id
        /// </summary>
        public UpsertCurveData(MarketDataIdentifier id)
        {
            ID = id;
        }

        /// <summary>
        /// The constructor with id and version
        /// </summary>
        public UpsertCurveData(MarketDataIdentifier id, LocalDateTime version)
        {
            ID = id;
            Version = version;
        }

        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public MarketDataIdentifier? ID { get; set; }

        /// <summary>
        /// The Version to operate on
        /// </summary>
        [MessagePack.Key(1)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// The timezone of the Rows. Must be the OriginalTimezone or, when Hourly, must be "UTC".
        /// </summary>
        [Required]
        [MessagePack.Key(2)]
        public string Timezone { get; set; }

        /// <summary>
        /// The UTC timestamp at which this assessment has been acquired/generated.
        /// </summary>
        [Required]
        [MessagePack.Key(3)]
        public Instant DownloadedAt { get; set; }

        /// <summary>
        /// The Market Data Identifier to upsert
        /// - LocalDateTime key is The Report timestamp in the MarketData OriginalTimezone but when Hourly must be UTC.
        /// - IDictionary value is The Market Data Identifier to upsert
        /// </summary>
        [MessagePack.Key(4)]
        public IDictionary<LocalDateTime, IDictionary<string, MarketAssessmentValue>>? MarketAssessment { get; set; }

        /// <summary>
        /// The timeserie data in OriginalTimezone or, when Hourly, must be UTC.
        /// </summary>
        [MessagePack.Key(5)]
        public IDictionary<LocalDateTime, double?> Rows { get; set; }

        /// <summary>
        /// Flag to choose between synchronous and asynchronous command execution
        /// </summary>
        [MessagePack.Key(6)]
        public bool DeferCommandExecution { get; set; } = true;

        /// <summary>
        /// Flag to choose between synchronous and asynchronous precomputed data generation
        /// </summary>
        [MessagePack.Key(7)]
        public bool DeferDataGeneration { get; set; } = true;

        /// <summary>
        /// Flag to choose between syncronoys and asyncronous precomputed data generation
        /// </summary>
        [MessagePack.Key(8)]
        public bool KeepNulls { get; set; } = false;

        /// <summary>
        /// The timeserie data in OriginalTimezone or, when Hourly, must be UTC.
        /// </summary>
        [MessagePack.Key(9)]
        public IDictionary<LocalDateTime, AuctionBids>? AuctionRows { get; set; }

        /// <summary>
        /// The BidAsk
        /// </summary>
        [MessagePack.Key(10)]
        public IDictionary<LocalDateTime, IDictionary<string, BidAskValue>>? BidAsk { get; set; }

        /// <summary>
        /// The Upsert Mode: Merge or Replace.
        /// Merge and replace are equivalent for ActualTimeSeries and Auction data for the other data
        /// types any previous data for version or product is replaced by the rows in the payload
        /// </summary>
        [MessagePack.Key(11)]
        public UpsertMode? UpsertMode { get; set; }
    }

    internal static class UpsertCurveDataExt
    {
        public static void Validate(this UpsertCurveData upsertCurveData)
        {
            if (upsertCurveData.ID == null)
                throw new ArgumentException("UpsertCurveData ID must be valorized", nameof(upsertCurveData));

            if (upsertCurveData.Timezone != null && DateTimeZoneProviders.Tzdb.GetZoneOrNull(upsertCurveData.Timezone) == null)
                throw new ArgumentException("UpsertCurveData Timezone must be in IANA database if valorized", nameof(upsertCurveData));

            if (upsertCurveData.DownloadedAt == default)
                throw new ArgumentException("UpsertCurveData DownloadedAt must be valorized", nameof(upsertCurveData));

            if (upsertCurveData.Rows == null)
            {
                if (upsertCurveData.Version != null)
                    throw new ArgumentException("UpsertCurveData Version must be NULL if Rows are NULL", nameof(upsertCurveData));

                if ((upsertCurveData.MarketAssessment == null || upsertCurveData.MarketAssessment.Count == 0) && (upsertCurveData.AuctionRows == null || upsertCurveData.AuctionRows.Count == 0) && (upsertCurveData.BidAsk == null || upsertCurveData.BidAsk.Count == 0))
                    throw new ArgumentException("UpsertCurveData MarketAssessment/Auctions/BidAsks must be valorized if Rows are NULL", nameof(upsertCurveData));
            }
            else
            {
                if (upsertCurveData.MarketAssessment != null)
                    throw new ArgumentException("UpsertCurveData MarketAssessment must be NULL if Rows are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.AuctionRows != null)
                    throw new ArgumentException("UpsertCurveData Auctions must be NULL if Rows are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.BidAsk != null)
                    throw new ArgumentException("UpsertCurveData BidAsk must be NULL if Rows are Valorized", nameof(upsertCurveData));

                foreach (var row in upsertCurveData.Rows)
                {
                    if (row.Key == default)
                        throw new ArgumentException($"Rows[{row}]: Invalid timepoint", nameof(upsertCurveData));
                }
            }

            if (upsertCurveData.MarketAssessment == null)
            {
                if ((upsertCurveData.Rows == null || upsertCurveData.Rows.Count == 0) && (upsertCurveData.AuctionRows == null || upsertCurveData.AuctionRows.Count == 0) && (upsertCurveData.BidAsk == null || upsertCurveData.BidAsk.Count == 0))
                    throw new ArgumentException("UpsertCurveData Rows/Auctions/BidAsks must be valorized if MarketAssesment are NULL", nameof(upsertCurveData));
            }
            else
            {
                if (upsertCurveData.Rows != null)
                    throw new ArgumentException("UpsertCurveData Rows must be NULL if MarketAssessment are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.AuctionRows != null)
                    throw new ArgumentException("UpsertCurveData Auctions must be NULL if MarketAssessment are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.BidAsk != null)
                    throw new ArgumentException("UpsertCurveData BidAsk must be NULL if MarketAssessment are Valorized", nameof(upsertCurveData));
            }

            if (upsertCurveData.AuctionRows == null)
            {
                if ((upsertCurveData.Rows == null || upsertCurveData.Rows.Count == 0) && (upsertCurveData.MarketAssessment == null || upsertCurveData.MarketAssessment.Count == 0) && (upsertCurveData.BidAsk == null || upsertCurveData.BidAsk.Count == 0))
                    throw new ArgumentException("UpsertCurveData Rows/MarketAssessment/BidAsks must be valorized if Auctions are NULL", nameof(upsertCurveData));
            }
            else
            {
                if (upsertCurveData.Rows != null)
                    throw new ArgumentException("UpsertCurveData Rows must be NULL if Auctions are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.MarketAssessment != null)
                    throw new ArgumentException("UpsertCurveData MarketAssesment must be NULL if Auctions are Valorized", nameof(upsertCurveData));

                if (upsertCurveData.BidAsk != null)
                    throw new ArgumentException("UpsertCurveData BidAsk must be NULL if Auctions are Valorized", nameof(upsertCurveData));
            }
        }
    }
}