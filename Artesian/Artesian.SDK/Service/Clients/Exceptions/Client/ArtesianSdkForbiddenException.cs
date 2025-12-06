// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Remote Exception
    /// </summary>
    public class ArtesianSdkForbiddenException : Exception
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ArtesianSdkProblemDetail ProblemDetail { get; } = null!;
        public ArtesianSdkForbiddenException(string message)
            : base(message)
        {
        }

        public ArtesianSdkForbiddenException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        public ArtesianSdkForbiddenException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public ArtesianSdkForbiddenException(string message, ArtesianSdkProblemDetail problemDetail)
         : base(message)
        {
            this.ProblemDetail = problemDetail;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
