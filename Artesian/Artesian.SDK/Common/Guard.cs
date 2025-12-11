using System;
using System.Runtime.CompilerServices;

namespace Artesian.SDK.Common
{
    internal static class Guard
    {
        public static T IsNotNull<T>(T? item, [CallerArgumentExpression(nameof(item))] string? expression = default, string? message = null)
            where T : class
        {
            if (item == null)
                throw new ArgumentNullException(
                    expression ?? "<unknown>",
                    message ?? (expression != null ? "Requires " + expression + " != null." : "IsNotNull() failed."));

            return item;
        }

        public static void IsTrue(bool value,
            [CallerArgumentExpression(nameof(value))] string? expression = null, string? message = null)
        {
            if (!value)
                throw new ArgumentException(
                    expression ?? "<unknown>",
                    message ?? (expression != null ? "Requires " + expression + " == true." : "IsTrue() failed."));
        }

        public static void IsNotNullOrWhiteSpace(string? value, [CallerArgumentExpression(nameof(value))] string? expression = null, string? message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    expression ?? "<unknown>",
                    message ?? (expression != null ? "Requires " + expression + " to not be blank." : "IsNotNullOrWhiteSpace() failed."));
        }

    }
}
