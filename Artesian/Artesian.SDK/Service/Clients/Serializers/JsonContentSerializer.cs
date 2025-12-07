// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Artesian.SDK.Service
{
    /// <summary>
    /// JSON content serializer using Newtonsoft.Json
    /// </summary>
    internal sealed class JsonContentSerializer : IContentSerializer
    {
        private readonly JsonSerializer? _serializer;

        /// <inheritdoc/>
        public string MediaType => "application/json";

        /// <summary>
        /// Initializes a new instance of the JsonContentSerializer class
        /// </summary>
        /// <param name="settings">JSON serializer settings</param>
        public JsonContentSerializer(JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _serializer = JsonSerializer.Create(settings);
        }

        /// <inheritdoc/>
        public bool CanSerialize<T>() => true;

        /// <inheritdoc/>
        public bool CanDeserialize<T>() => true;

        /// <inheritdoc/>
        public Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

#pragma warning disable MA0042 // Prefer using 'await using'
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                _serializer.Serialize(jsonWriter, value);
            }
#pragma warning restore MA0042 // Prefer using 'await using'

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

#pragma warning disable MA0042 // Prefer using 'await using'
            using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 1024, leaveOpen: true))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var result = _serializer.Deserialize<T>(jsonReader);
                return Task.FromResult(result);
            }
#pragma warning restore MA0042 // Prefer using 'await using'
        }
    }
}
