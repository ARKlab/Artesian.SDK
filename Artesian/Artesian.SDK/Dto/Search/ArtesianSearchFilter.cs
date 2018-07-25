﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

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

    public static class ArtesianSearchFilterExt
    {
        public static void Validate(this ArtesianSearchFilter artesianSearchFilter)
        {
            var validSorts = @"^(MarketDataId|ProviderName|MarketDataName|OriginalGranularity|Type|OriginalTimezone|Created|LastUpdated)( (asc|desc))?$";

            if (artesianSearchFilter.SearchText == null)
                throw new ArgumentNullException(nameof(artesianSearchFilter.SearchText));

            if (artesianSearchFilter.Sorts != null)
            {
                foreach (string element in artesianSearchFilter.Sorts)
                {
                    if (element.Equals(validSorts))
                        throw new ArgumentException("Invalid search params");
                }
            }

            if (artesianSearchFilter.PageSize < 0)
                throw new ArgumentException("Page size is less than 0");

            if (artesianSearchFilter.Page < 0)
                throw new ArgumentException("Page is less than 0");

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
