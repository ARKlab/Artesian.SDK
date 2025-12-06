// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using Artesian.SDK.Service;
using MessagePack;
using Newtonsoft.Json;
using System;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The TimeTransform base entity with Etag
    /// </summary>
    [MessagePackObject]
    [Union(0, typeof(TimeTransformSimpleShift))]
    [JsonConverter(typeof(TimeTransformConverter))]
    public abstract class TimeTransform
    {
        /// <summary>
        /// The Time transform Identifier
        /// </summary>
        [Key("ID")]
        public int ID { get; set; }
        /// <summary>
        /// The Time transform Name
        /// </summary>
        [Key("Name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The Time transform Etag
        /// </summary>
        [Key("Etag")]
        public Guid ETag { get; set; } = null!;

        /// <summary>
        /// The information regarding who defined a time transformation
        /// </summary>
        [Key("DefinedBy")]
        public TransformDefinitionType DefinedBy { get; set; } = null!;


        /// <summary>
        /// The Transform Type
        /// </summary>
        [IgnoreMember]
        public abstract TransformType Type { get; }

    }
}
