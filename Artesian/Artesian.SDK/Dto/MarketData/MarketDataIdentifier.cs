// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Common;
using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The MarketData identifier entity
    /// </summary>
    [MessagePackObject]
    public record MarketDataIdentifier
    {
        /// <summary>
        /// The MarketData identifier constructor by provider and name
        /// </summary>
        public MarketDataIdentifier(string provider, string name)
        {
            Provider = provider;
            Name = name;
        }

        /// <summary>
        /// The Provider unique name
        /// </summary>
        [Required]
        [MessagePack.Key(0)]
        public string Provider { get; protected set; }

        /// <summary>
        /// The Market Data unique name for the Provider
        /// </summary>
        [Required]
        [MessagePack.Key(1)]
        public string Name { get; protected set; }

        /// <summary>
        /// The Market Data override for ToString()
        /// </summary>
        public override string ToString()
        {
            return string.Format("Provider: {0} Name: {1}", Provider, Name);
        }

    }

    internal static class MarketDataIdentifierExt
    {
        public static void Validate(this MarketDataIdentifier marketDataIdentifier)
        {
            ArtesianUtils.IsValidProvider(marketDataIdentifier.Provider,1,50);
            ArtesianUtils.IsValidMarketDataName(marketDataIdentifier.Name,1,250);
        }
    }

}
