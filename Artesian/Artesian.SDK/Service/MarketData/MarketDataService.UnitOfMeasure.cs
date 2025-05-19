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
        /// Check the conversion between the input units of measure and the target unit of measure
        /// </summary>
        /// <param name="inputUnitsOfMeasure">Input units of measure</param>
        /// <param name="targetUnitOfMeasure">target unit of measure</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>The CheckConversionResult class.
        ///             TargetUnitOfMeasure = the target unit of measure
        ///             ConvertibleInputUnitsOfMeasure = list of convertible input units of measure
        ///             NotConvertibleInputUnitsOfMeasure = list of not convertible input units of measure</returns>
        public Task<CheckConversionResult> CheckConversionAsync(string[] inputUnitsOfMeasure, string targetUnitOfMeasure, CancellationToken ctk = default)
        {
            if (inputUnitsOfMeasure.Length == 0)
                throw new ArgumentException("InputUnitsOfMeasure has no elements", nameof(inputUnitsOfMeasure));

            var url = "/uom/checkconversion"
                .SetQueryParam("inputUnitsOfMeasure", inputUnitsOfMeasure)
                .SetQueryParam("targetUnitOfMeasure", targetUnitOfMeasure);

            return _client.Exec<CheckConversionResult>(HttpMethod.Get, url, ctk: ctk);
        }
    }
}
