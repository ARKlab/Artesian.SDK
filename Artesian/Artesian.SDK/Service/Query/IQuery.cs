// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
namespace Artesian.SDK.Service
{
    interface IQuery<T>
    {
        T ForMarketData(int[] ids);
        T ForFilterId(int filterId);
        T InTimezone(string tz);
    }
}