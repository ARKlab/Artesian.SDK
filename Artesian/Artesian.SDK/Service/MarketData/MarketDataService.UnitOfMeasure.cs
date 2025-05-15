// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System;
using Flurl;
using Artesian.SDK.Dto.UoM;

namespace Artesian.SDK.Service
{
    public partial class MarketDataService : IMarketDataService
    {
        /// <summary>
        /// Check the conversion between the input unit of measures and the target unit of measure
        /// </summary>
        /// <param name="inputUnitOfMeasures">Input unit of measures</param>
        /// <param name="targetUnitOfMeasure">target unit of measure</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>CheckConversionResults Entity</returns>
        public Task<CheckConversionResults> CheckConversion(string[] inputUnitOfMeasures, string targetUnitOfMeasure, CancellationToken ctk = default)
        {
            if (inputUnitOfMeasures.Length == 0)
                throw new ArgumentException("InputUnitOfMeasures has no elements", nameof(inputUnitOfMeasures));

            var url = "/uom/checkconversion"
                .SetQueryParam("inputUnitOfMeasures", inputUnitOfMeasures)
                .SetQueryParam("targetUnitOfMeasure", targetUnitOfMeasure);

            return _client.Exec<CheckConversionResults>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
