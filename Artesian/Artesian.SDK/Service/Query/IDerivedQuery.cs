// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Dto;

using NodaTime;

namespace Artesian.SDK.Service
{
    interface IDerivedQuery<T>: IQueryWithFill<T>, IQueryWithExtractionInterval<T>, IQueryWithExtractionRange<T>
    {
        T InGranularity(Granularity granularity);
        T ForLastNVersions(int lastN);
        T ForMUV(LocalDateTime? versionLimit = null);
        T ForMostRecent();
        T ForMostRecent(LocalDate start, LocalDate end);
        T ForMostRecent(Period lastOfPeriod);
        T ForMostRecent(Period from, Period to);
        T ForLastOfDays(LocalDate start, LocalDate end);
        T ForLastOfDays(Period lastOfPeriod);
        T ForLastOfDays(Period from, Period to);
        T ForLastOfMonths(LocalDate start, LocalDate end);
        T ForLastOfMonths(Period lastOfPeriod);
        T ForLastOfMonths(Period from, Period to);
        T ForVersion(LocalDateTime version);
        T WithTimeTransform(int tr);
        T WithTimeTransform(SystemTimeTransform tr);
        T WithFillCustomValue(double value);
    }
}