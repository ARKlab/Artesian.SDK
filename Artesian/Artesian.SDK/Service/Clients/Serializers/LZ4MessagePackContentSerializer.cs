// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using MessagePack;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace Artesian.SDK.Service
{
    /// <summary>
    /// LZ4-compressed MessagePack content serializer
    /// </summary>
    internal sealed class LZ4MessagePackContentSerializer : IContentSerializer
    {
        private readonly MessagePackSerializerOptions _options;

        /// <inheritdoc/>
        public string MediaType => "application/x.msgpacklz4";

        /// <summary>
        /// Initializes a new instance of the LZ4MessagePackContentSerializer class
        /// </summary>
        /// <param name="resolver">MessagePack formatter resolver</param>
        public LZ4MessagePackContentSerializer(IFormatterResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            _options = MessagePackSerializer.DefaultOptions.WithResolver(resolver).WithCompression(MessagePackCompression.Lz4Block);
        }

        /// <inheritdoc/>
        public bool CanSerialize<T>()
        {
            return _options.Resolver.GetFormatterDynamic(typeof(T)) != null;
        }

        /// <inheritdoc/>
        public bool CanDeserialize<T>()
        {
            return _options.Resolver.GetFormatterDynamic(typeof(T)) != null;
        }

        /// <inheritdoc/>
        public Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return MessagePackSerializer.SerializeAsync(stream, value, _options, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<T?> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return await MessagePackSerializer.DeserializeAsync<T>(stream, _options, cancellationToken).ConfigureAwait(false);
        }
    }
}
