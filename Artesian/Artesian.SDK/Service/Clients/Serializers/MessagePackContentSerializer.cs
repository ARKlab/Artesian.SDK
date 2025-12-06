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
    /// MessagePack content serializer
    /// </summary>
    internal sealed class MessagePackContentSerializer : IContentSerializer
    {
        private readonly MessagePackSerializerOptions _options;

        /// <inheritdoc/>
        public string MediaType => "application/x-msgpack";

        /// <summary>
        /// Initializes a new instance of the MessagePackContentSerializer class
        /// </summary>
        /// <param name="resolver">MessagePack formatter resolver</param>
        public MessagePackContentSerializer(IFormatterResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            _options = MessagePackSerializer.DefaultOptions.WithResolver(resolver);
        }

        /// <inheritdoc/>
        public bool CanSerialize(Type type)
        {
            return _options.Resolver.GetFormatterDynamic(type) != null;
        }

        /// <inheritdoc/>
        public bool CanDeserialize(Type type)
        {
            return _options.Resolver.GetFormatterDynamic(type) != null;
        }

        /// <inheritdoc/>
        public Task SerializeAsync(Type type, object? value, Stream stream, CancellationToken cancellationToken = default)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return MessagePackSerializer.SerializeAsync(type, stream, value, _options, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<object?> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return await MessagePackSerializer.DeserializeAsync(type, stream, _options, cancellationToken).ConfigureAwait(false);
        }
    }
}
