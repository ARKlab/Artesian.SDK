﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Artesian.SDK.Dto;
using Flurl;
using NodaTime;
using System;
using System.Collections.Generic;

namespace Artesian.SDK.Service
{
    public partial class MetadataService : IMetadataService
    {
        /// <summary>
        /// Read a time transform entity from the service by ID
        /// </summary>
        /// <param name="timeTransformId">ID of the time transform to be retrieved</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<TimeTransform> ReadTimeTransformBaseAsync(int timeTransformId, CancellationToken ctk = default)
        {
            if (timeTransformId < 1)
                throw new ArgumentException("Transform id is invalid : " + timeTransformId);

            return _client.Exec<TimeTransform>(HttpMethod.Get, $@"/timeTransform/entity/{timeTransformId}", ctk: ctk);
        }
        /// <summary>
        /// Read a paged set of time transform entities from the service
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="userDefined">Retrieve either user or system defined time transforms</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<PagedResult<TimeTransform>> ReadTimeTransformsAsync(int page, int pageSize, bool userDefined, CancellationToken ctk = default)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and Page number need to be greater than 0. Page:" + page + " Page Size:" + pageSize);

            var url = "/timeTransform/entity"
                    .SetQueryParam("pageSize", pageSize)
                    .SetQueryParam("page", page)
                    .SetQueryParam("userDefined", userDefined)
                    ;

            return _client.Exec<PagedResult<TimeTransform>>(HttpMethod.Get, url, ctk: ctk);
        }
        /// <summary>
        /// Register a new TimeTransform
        /// </summary>
        /// <param name="timeTransform">the entity we are going to insert</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<TimeTransform> RegisterTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default)
        {
            (timeTransform as TimeTransformSimpleShift).Validate();

            var url = "/timeTransform/entity";

            return _client.Exec<TimeTransform, TimeTransform>(HttpMethod.Post, url, timeTransform, ctk: ctk);
        }
        /// <summary>
        /// Update the TimeTransform
        /// </summary>
        /// <param name="timeTransform">the entity we are going to update</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task<TimeTransform> UpdateTimeTransformBaseAsync(TimeTransform timeTransform, CancellationToken ctk = default)
        {
            (timeTransform as TimeTransformSimpleShift).Validate();

            var url = "/timeTransform/entity".AppendPathSegment(timeTransform.ID);

            return _client.Exec<TimeTransform, TimeTransform>(HttpMethod.Put, url, timeTransform, ctk: ctk);
        }
        /// <summary>
        /// Delete the TimeTransform
        /// </summary>
        /// <param name="timeTransformId">the entity id we are going to delete</param>
        /// <param name="ctk">CancellationToken</param>
        /// <returns>client.Exec() <see cref="Client.Exec{TResult}(HttpMethod, string, CancellationToken)"/></returns>
        public Task DeleteTimeTransformSimpleShiftAsync(int timeTransformId, CancellationToken ctk = default)
        {
            var url = "/timeTransform/entity".AppendPathSegment(timeTransformId);

            return _client.Exec(HttpMethod.Delete, url, ctk: ctk);
        }

    }
}
