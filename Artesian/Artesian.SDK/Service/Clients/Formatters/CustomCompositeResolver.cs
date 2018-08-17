﻿// Copyright (c) ARK LTD. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for
// license information. 
using MessagePack;
using MessagePack.Formatters;
using MessagePack.NodaTime;
using MessagePack.Resolvers;

namespace Artesian.SDK.Service
{
    internal class CustomCompositeResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new CustomCompositeResolver();

        static readonly IFormatterResolver[] resolvers = new[]
        {
            BuiltinResolver.Instance,
            NodatimeResolver.Instance,
            AttributeFormatterResolver.Instance,
            DynamicEnumAsStringResolver.Instance,
            StandardResolver.Instance
        };

        CustomCompositeResolver()
        {
        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                foreach (var item in resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        formatter = f;
                        return;
                    }
                }
            }
        }
    }
}
