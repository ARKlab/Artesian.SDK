// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using MessagePack;
using System;
using System.Text.Json.Serialization;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The TimeTransform base entity with Etag
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(TimeTransformSimpleShift))]
    [Newtonsoft.Json.JsonConverter(typeof(TimeTransformConverter))]
    [JsonConverter(typeof(TimeTransformConverterSTJ))]
    public abstract record TimeTransform
    {
        /// <summary>
        /// The Time transform Identifier
        /// </summary>
        [Key("ID")]
        public int ID { get; init; }
        /// <summary>
        /// The Time transform Name
        /// </summary>
        [Key("Name")]
        public required string Name { get; init; }
        /// <summary>
        /// The Time transform Etag
        /// </summary>
        [Key("Etag")]
        public Guid ETag { get; init; }
        /// <summary>
        /// The information regarding who defined a time transformation
        /// </summary>
        [Key("DefinedBy")]
        public TransformDefinitionType DefinedBy { get; init; }

        /// <summary>
        /// The Transform Type
        /// </summary>
        [IgnoreMember]
        public abstract TransformType Type { get; }

    }
}
