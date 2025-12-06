using MessagePack;

using NodaTime;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The curve data for a delete command.
    /// </summary>
    [MessagePackObject]
    public class DeleteCurveData
    {
        /// <summary>
        /// The default constructor
        /// </summary>
        public DeleteCurveData()
        {
        }

        /// <summary>
        /// The constructor with id
        /// </summary>
        public DeleteCurveData(MarketDataIdentifier id)
        {
            ID = id;
        }

        /// <summary>
        /// The constructor with id and version
        /// </summary>
        public DeleteCurveData(MarketDataIdentifier id, LocalDateTime version)
        {
            ID = id;
            Version = version;
        }

        /// <summary>
        /// The Market Data Identifier
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public MarketDataIdentifier ID { get; set; } = null!;


        /// <summary>
        /// The Version to operate on
        /// </summary>
        [MessagePack.Key(1)]
        public LocalDateTime Version { get; set; } = default;

        /// <summary>
        /// For DateSeries if provided must be equal to MarketData OrignalTimezone Default:MarketData OrignalTimezone. For TimeSeries Default:CET
        /// </summary>
        [Required]
        [MessagePack.Key(2)]
        public string Timezone { get; set; } = null!;


        /// <summary>
        /// Start date of range to be deleted  
        /// </summary>
        [Required]
        [MessagePack.Key(3)]
        public LocalDateTime RangeStart { get; set; } = default;


        /// <summary>
        /// End date of range to be deleted   
        /// </summary>
        [Required]
        [MessagePack.Key(4)]
        public LocalDateTime RangeEnd { get; set; } = default;


        /// <summary>
        /// The list of Product. Only *, is special character for 'delete all products in the range'
        /// </summary>
        [MessagePack.Key(5)]
        public IList<string> Product { get; set; } = null!;


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
    }

    internal static class DeleteCurveDataExt
    {
        public static void Validate(this DeleteCurveData deleteCurveData)
        {
            if (deleteCurveData.ID == null)
                throw new ArgumentException("DeleteCurveData ID must be valorized", nameof(deleteCurveData));

            if (deleteCurveData.Timezone != null && DateTimeZoneProviders.Tzdb.GetZoneOrNull(deleteCurveData.Timezone) == null)
                throw new ArgumentException("DeleteCurveData Timezone must be in IANA database if valorized", nameof(deleteCurveData));
        }
    }
}