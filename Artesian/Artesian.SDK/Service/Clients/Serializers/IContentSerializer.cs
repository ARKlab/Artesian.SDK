// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Interface for content serialization and deserialization
    /// </summary>
    internal interface IContentSerializer
    {
        /// <summary>
        /// Gets the media type supported by this serializer
        /// </summary>
        string MediaType { get; }

        /// <summary>
        /// Determines if this serializer can serialize the specified type
        /// </summary>
        bool CanSerialize<T>();

        /// <summary>
        /// Determines if this serializer can deserialize the specified type
        /// </summary>
        bool CanDeserialize<T>();

        /// <summary>
        /// Serializes an object to a stream
        /// </summary>
        Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deserializes an object from a stream
        /// </summary>
        Task<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default);
    }
}
