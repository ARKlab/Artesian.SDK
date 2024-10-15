// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The MarketData Type
    /// </summary>
    public enum MarketDataType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
          ActualTimeSerie
        , VersionedTimeSerie
        , MarketAssessment
        , Auction
        , BidAsk
        , DerivedTimeSerie
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
