using Artesian.SDK.Common;

using MessagePack;

using NodaTime;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operations to be executed on a list of ids
    /// </summary>
    [MessagePackObject]
    public class Operations
    {
        /// <summary>
        /// The Operations constructor
        /// </summary>
        public Operations()
        {
            IDS = new HashSet<MarketDataETag>();
            OperationList = new List<OperationParams>();
        }

        /// <summary>
        /// The Market Data Identifiers
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public ISet<MarketDataETag> IDS { get; set; }

        /// <summary>
        /// The Operations to be executed
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public IList<OperationParams> OperationList { get; set; }
    }


    internal static class OperationsExt
    {
        public static void Validate(this Operations operations)
        {
            foreach (var id in operations.IDS)
            {
                if (id == null)
                    throw new ArgumentException("Operations: any single ID must be valorized", nameof(operations));

                if (String.IsNullOrWhiteSpace(id.ETag))
                    throw new ArgumentException("Operations: any single ETAG must be valorized", nameof(operations));
            }

            foreach (var op in operations.OperationList)
            {
                if (op == null)
                    throw new ArgumentException("Operations: any single operation must be valorized", nameof(operations));

                if (op.Params == null)
                    throw new ArgumentException("Operations: any single Params in operationList must be valorized", nameof(operations));

                switch (op.Type)
                {
                    case OperationType.EnableTag:
                        {
                            var p = op.Params as OperationEnableDisableTag;

                            if (!p.TagKey._isValidTagKey())
                                throw new ArgumentException("Operations: any single Params TagKey must have specific values", nameof(operations));

                            ArtesianUtils.IsValidString(p.TagKey, 3, 50);

                            ArtesianUtils.IsValidString(p.TagValue, 1, 50);

                            break;
                        }
                    case OperationType.DisableTag:
                        {
                            var p = op.Params as OperationEnableDisableTag;

                            if (!p.TagKey._isValidTagKey())
                                throw new ArgumentException("Operations: any single Params TagKey must have specific values", nameof(operations));

                            ArtesianUtils.IsValidString(p.TagKey, 3, 50);

                            ArtesianUtils.IsValidString(p.TagValue, 1, 50);

                            break;
                        }
                    case OperationType.UpdateTimeTransformID:
                        {
                            var p = op.Params as OperationUpdateTimeTransform;

                            break;
                        }
                    case OperationType.UpdateAggregationRule:
                        {
                            var p = op.Params as OperationUpdateAggregationRule;

                            break;
                        }
                    case OperationType.UpdateOriginalTimeZone:
                        {
                            if (op.Params is OperationUpdateOriginalTimeZone p)
                            {
                                if (!String.IsNullOrWhiteSpace(p.Value) && DateTimeZoneProviders.Tzdb.GetZoneOrNull(p.Value) == null)
                                    throw new ArgumentException("Operations: any single Params Value must be in IANA database if valorized", nameof(operations));
                            }
                            else
                                throw new InvalidOperationException("Operations: Data cannot be used as OperationUpdateOriginalTimeZone");

                            break;
                        }
                    case OperationType.UpdateProviderDescription:
                        {
                            var p = op.Params as OperationUpdateProviderDescription;

                            break;
                        }
                    default:
                        throw new NotSupportedException("Operations: The Operation Type is not supported");
                }
            }
        }

        private static bool _isValidTagKey(this string stringToEvaluate)
        {
            if (stringToEvaluate == "Type" ||
                stringToEvaluate == "ProviderName" ||
                stringToEvaluate == "MarketDataName" ||
                stringToEvaluate == "OriginalGranularity" ||
                stringToEvaluate == "OriginalGranularity" ||
                stringToEvaluate == "OriginalTimezone" ||
                stringToEvaluate == "MarketDataId" ||
                stringToEvaluate == "AggregationRule"
                )
                return false;

            return true;
        }
    }



}
