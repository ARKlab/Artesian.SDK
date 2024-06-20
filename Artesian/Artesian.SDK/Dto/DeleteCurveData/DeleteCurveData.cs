using Ark.Tools.Nodatime;

using MessagePack;
using NodaTime;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public MarketDataIdentifier ID { get; set; }

        /// <summary>
        /// The Version to operate on
        /// </summary>
        [MessagePack.Key(1)]
        public LocalDateTime? Version { get; set; }

        /// <summary>
        /// The timezone of the Range to delete. It is applied only if the curve Granularity is less than Day. Default is CET
        /// </summary>
        [Required]
        [MessagePack.Key(2)]
        public string Timezone { get; set; }

        /// <summary>
        /// Start date of range to be deleted  
        /// </summary>
        [Required]
        [MessagePack.Key(3)]
        public LocalDateTime RangeStart { get; set; }

        /// <summary>
        /// End date of range to be deleted   
        /// </summary>
        [Required]
        [MessagePack.Key(4)]
        public LocalDateTime RangeEnd { get; set; }

        /// <summary>
        /// The list of Product. Only *, is special character for 'delete all products in the range'
        /// </summary>
        [MessagePack.Key(5)]
        public IList<string> Product { get; set; }

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
                throw new ArgumentException("DeleteCurveData ID must be valorized");

            if (deleteCurveData.Timezone != null && DateTimeZoneProviders.Tzdb.GetZoneOrNull(deleteCurveData.Timezone) == null)
                throw new ArgumentException("DeleteCurveData Timezone must be in IANA database if valorized");
        }
    }
}