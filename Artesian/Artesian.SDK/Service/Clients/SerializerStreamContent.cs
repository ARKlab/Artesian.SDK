// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information.
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// HttpContent that serializes data directly to the output stream without buffering
    /// </summary>
    internal sealed class SerializerStreamContent : HttpContent
    {
        private readonly IContentSerializer _serializer;
        private readonly Type _type;
        private readonly object _value;
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Initializes a new instance of SerializerStreamContent
        /// </summary>
        /// <param name="serializer">The content serializer to use</param>
        /// <param name="type">The type of the value to serialize</param>
        /// <param name="value">The value to serialize</param>
        /// <param name="cancellationToken">Cancellation token for the serialization</param>
        public SerializerStreamContent(IContentSerializer serializer, Type type, object value, CancellationToken cancellationToken = default)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _value = value;
            _cancellationToken = cancellationToken;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await _serializer.SerializeAsync(_type, _value, stream, _cancellationToken).ConfigureAwait(false);
        }

        protected override bool TryComputeLength(out long length)
        {
            // We don't know the length ahead of time
            length = -1;
            return false;
        }

#if NET5_0_OR_GREATER
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context, CancellationToken cancellationToken)
        {
            return _serializer.SerializeAsync(_type, _value, stream, cancellationToken);
        }
#endif
    }
}
