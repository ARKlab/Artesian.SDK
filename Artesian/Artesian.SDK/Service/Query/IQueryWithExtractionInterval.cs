// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
namespace Artesian.SDK.Service
{
    interface IQueryWithExtractionInterval<T>: IQuery<T>
    {
        T InRelativeInterval(RelativeInterval relativeInterval);
    }
}