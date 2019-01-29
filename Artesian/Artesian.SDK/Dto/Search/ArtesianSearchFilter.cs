﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The dto for a new search facet based
    /// </summary>
    public class ArtesianSearchFilter
    {
        /// <summary>
        /// Free search text
        /// </summary>
        [Required]
        public string SearchText { get; set; }
        /// <summary>
        /// Filter by facet name, facet values
        /// </summary>
        public IDictionary<string, string[]> Filters { get; set; }
        /// <summary>
        /// sort by field name
        /// </summary>
        public IList<string> Sorts { get; set; }
        /// <summary>
        /// page size
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// page
        /// </summary>
        [Required]
        public int Page { get; set; }
    }

    internal static class ArtesianSearchFilterExt
    {
        public static void Validate(this ArtesianSearchFilter artesianSearchFilter)
        {
            var validSorts = new Regex("^(MarketDataId|ProviderName|MarketDataName|OriginalGranularity|Type|OriginalTimezone|Created|LastUpdated)( (asc|desc))?$");

            if (artesianSearchFilter.Sorts != null)
            {
                foreach (string element in artesianSearchFilter.Sorts)
                {
                    if (!validSorts.IsMatch(element))
                        throw new ArgumentException($"Invalid search param {element}");
                }
            }

            if (artesianSearchFilter.PageSize <= 0)
                throw new ArgumentException("Page size should be greater than 0");

            if (artesianSearchFilter.Page <= 0)
                throw new ArgumentException("Page should be greater than 0");

            if (artesianSearchFilter.Filters!=null) {
                foreach (KeyValuePair<string, string[]> element in artesianSearchFilter.Filters)
                {
                    ArtesianUtils.IsValidString(element.Key, 3, 50);

                    if (element.Value != null)
                    {
                        if (element.Key != "MarketDataName")
                        {
                            foreach (string value in element.Value)
                            {
                                ArtesianUtils.IsValidString(value, 1, 50);
                            }

                        }
                        else if (element.Key == "MarketDataName")
                        {
                            foreach (string value in element.Value)
                            {
                                ArtesianUtils.IsValidString(value, 1, 250);
                            }
                        }
                    }
                }
            }
        }
    }
}
