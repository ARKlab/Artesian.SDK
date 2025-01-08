namespace Artesian.SDK.Common
{
    /// <summary>
    /// DatePeriod enums
    /// </summary>
    internal enum DatePeriod : byte
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Day = 2,
        Week = 3,
        Month = 4,
        Bimestral = 5,
        Trimestral = 6,
        Calendar = 7
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
