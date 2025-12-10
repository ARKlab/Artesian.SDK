// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Validation Exception
    /// </summary>
    public class ArtesianSdkValidationException : ArtesianSdkRemoteException
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ArtesianSdkValidationException(string message)
            : base(message)
        {
        }

        public ArtesianSdkValidationException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        public ArtesianSdkValidationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
        public ArtesianSdkValidationException(string message, ArtesianSdkProblemDetail? problemDetail)
           : base(message,problemDetail)
        {
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
