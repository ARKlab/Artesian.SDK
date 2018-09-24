using Artesian.SDK.Service;
using MessagePack;
using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The Operations to be executed on a list of ids
    /// </summary>
    [MessagePackObject]
    public class Operations
    {
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


    public static class OperationsExt
    {
        public static void Validate(this Operations operations)
        {
            foreach (var id in operations.IDS)
            {
                if (id == null)
                    throw new ArgumentException("Operations: any single ID must be valorized");
                else
                {
                    if (String.IsNullOrWhiteSpace(id.ETag))
                        throw new ArgumentException("Operations: any single ETAG must be valorized");
                }
            }

            foreach (var op in operations.OperationList)
            {
                if (op == null)
                    throw new ArgumentException("Operations any single operation must be valorized");
                else
                {
                    if (op.Params == null)
                        throw new ArgumentException("Operations: any single Params in operationList must be valorized");

                    switch (op.Type)
                    {
                        case OperationType.EnableTag:
                            {
                                var p = op.Params as OperationEnableDisableTag;

                                if (!p.TagKey.IsValidTagKey())
                                    throw new ArgumentException("Operations: any single Params TagKey must have specific values");

                                if (!p.TagKey.IsValidString(3, 50))
                                    throw new ArgumentException("Operations: any single Params TagKey must match internal format");

                                if (!p.TagValue.IsValidString(1, 50))
                                    throw new ArgumentException("Operations: any single Params TagValue must match internal format");

                                break;
                            }
                        case OperationType.DisableTag:
                            {
                                var p = op.Params as OperationEnableDisableTag;

                                if (!p.TagKey.IsValidTagKey())
                                    throw new ArgumentException("Operations: any single Params TagKey must have specific values");

                                if (!p.TagKey.IsValidString(3, 50))
                                    throw new ArgumentException("Operations: any single Params TagKey must match internal format");

                                if (!p.TagValue.IsValidString(1, 50))
                                    throw new ArgumentException("Operations: any single Params TagValue must match internal format");

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
                                var p = op.Params as OperationUpdateOriginalTimeZone;

                                if (!String.IsNullOrWhiteSpace(p.Value) && DateTimeZoneProviders.Tzdb.GetZoneOrNull(p.Value) == null)
                                    throw new ArgumentException("Operations: any single Params Value must be in IANA database if valorized");

                                break;
                            }
                        default:
                            throw new ArgumentException("Operations: any single Params TagKey must have specific values");
                    }
                }
            }
        }

        private static bool IsValidString(this string stringToEvaluate, int minLenght, int maxLenght)
        {
            if (stringToEvaluate != null && (stringToEvaluate.Length > minLenght && stringToEvaluate.Length < maxLenght) && Regex.IsMatch(stringToEvaluate, ArtesianConstants.CharacterValidatorRegEx))
            {
                return true;
            }
            else
                return false;
        }

        private static bool IsValidTagKey(this string stringToEvaluate)
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
            else
                return true;
        }
    }



}
