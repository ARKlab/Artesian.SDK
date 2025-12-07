// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Query Paramaters DTO
    /// </summary>
    public abstract class QueryParamaters
    {
        /// <summary>
        /// 
        /// </summary>
        protected QueryParamaters()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="timezone"></param>
        /// <param name="filterId"></param>
        protected QueryParamaters(
            IEnumerable<int> ids,
            string timezone, 
            int? filterId
            )
        {
            this.Ids = ids;
            this.TimeZone = timezone;
            this.FilterId = filterId;
        }

        /// <summary>
        /// IDs
        /// </summary>
        public IEnumerable<int>? Ids { get; set; }
        /// <summary>
        /// Timezone
        /// </summary>
        public string? TimeZone { get; set; }
        /// <summary>
        /// FilterId
        /// </summary>
        public int? FilterId { get; set; }

    }
}
