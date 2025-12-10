// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using System;

namespace Artesian.SDK.Service
{
    /// <summary>
    /// Concurrency Exception
    /// </summary>
    public class ArtesianSdkOptimisticConcurrencyException : ArtesianSdkRemoteException
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ArtesianSdkOptimisticConcurrencyException(string message)
            : base(message)
        {
        }

        public ArtesianSdkOptimisticConcurrencyException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }

        public ArtesianSdkOptimisticConcurrencyException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public ArtesianSdkOptimisticConcurrencyException(string message, ArtesianSdkProblemDetail? problemDetail)
          : base(message, problemDetail)
        {
        }  
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
