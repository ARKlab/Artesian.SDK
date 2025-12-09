// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Artesian.SDK.Dto;
using Artesian.SDK.Dto.Serialize;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// System.Text.Json converter for TimeTransform polymorphic deserialization
    /// </summary>
    internal sealed class TimeTransformConverterSTJ : JsonPolymorphicConverter<TimeTransform, TransformType>
    {
        public TimeTransformConverterSTJ()
            : base(nameof(TimeTransform.Type))
        {
        }

        protected override Type GetType(TransformType discriminatorValue)
        {
            return discriminatorValue switch
            {
                TransformType.SimpleShift => typeof(TimeTransformSimpleShift),
                TransformType.Calendar => throw new NotSupportedException($"Not yet implemented transform {TransformType.Calendar}"),
                TransformType.Composition => throw new NotSupportedException($"Not yet implemented transform {TransformType.Composition}"),
                _ => throw new InvalidOperationException($"Can't deserialize TimeTransform. TransformType '{discriminatorValue}' is not valid.")
            };
        }
    }
}
