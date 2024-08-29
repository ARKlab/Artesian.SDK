// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

namespace Artesian.SDK.Service
{
    interface IDerivedQuery<T>: IQueryWithFill<T>, IQueryWithExtractionInterval<T>, IQueryWithExtractionRange<T>
    {
        T InGranularity(Granularity granularity);
        T WithTimeTransform(int tr);
        T WithTimeTransform(SystemTimeTransform tr);
        T WithFillCustomValue(double value);
    }
}