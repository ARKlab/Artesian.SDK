// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// JSON content serializer using System.Text.Json
    /// </summary>
    internal sealed class SystemTextJsonContentSerializer : IContentSerializer
    {
        private readonly JsonSerializerOptions _options;

        /// <inheritdoc/>
        public string MediaType => "application/json";

        /// <summary>
        /// Initializes a new instance of the SystemTextJsonContentSerializer class
        /// </summary>
        /// <param name="options">JSON serializer options</param>
        public SystemTextJsonContentSerializer(JsonSerializerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _options = options;
        }

        /// <inheritdoc/>
        public bool CanSerialize<T>() => true;

        /// <inheritdoc/>
        public bool CanDeserialize<T>() => true;

        /// <inheritdoc/>
        public async Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            await JsonSerializer.SerializeAsync(stream, value, _options, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return await JsonSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);
        }
    }
}
