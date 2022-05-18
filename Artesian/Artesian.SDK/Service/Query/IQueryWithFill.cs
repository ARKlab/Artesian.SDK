// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using NodaTime;

namespace Artesian.SDK.Service
{
    interface IQueryWithFill<T>: IQuery<T>
    {
        T WithFillNull();
        T WithFillLatestValue(Period period, bool fillerContinue);
        T WithFillNone();
    }
}