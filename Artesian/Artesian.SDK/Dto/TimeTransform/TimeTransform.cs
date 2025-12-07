using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// System TimeTransform enum
    /// </summary>
    public enum SystemTimeTransform
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        GASDAY66 = 1,
        THERMALYEAR = 2,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// System TimeTransforms
    /// </summary>
    public static class SystemTimeTransforms
    {
        /// <summary>
        /// System TimeTransform GASDAY66
        /// </summary>
        public static readonly TimeTransformSimpleShift GASDAY66 = new TimeTransformSimpleShift()
        {
            ID = 1,
            Name = SystemTimeTransform.GASDAY66.ToString(),
            ETag = Guid.Empty,
            DefinedBy = TransformDefinitionType.System,
            Period = Granularity.Day,
            PositiveShift = "PT6H",
            NegativeShift = "",
        };

        /// <summary>
        /// System TimeTransform THERMALYEAR
        /// </summary>
        public static readonly TimeTransformSimpleShift THERMALYEAR = new TimeTransformSimpleShift()
        {
            ID = 2,
            Name = SystemTimeTransform.THERMALYEAR.ToString(),
            ETag = Guid.Empty,
            DefinedBy = TransformDefinitionType.System,
            Period = Granularity.Year,
            PositiveShift = "",
            NegativeShift = "",
        };
    }
}
