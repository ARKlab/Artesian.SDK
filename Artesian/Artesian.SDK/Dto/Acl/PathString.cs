using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Artesian.SDK.Dto
{
    /// <summary>
    /// The PathString entity
    /// </summary>
    public class PathString : IEquatable<PathString>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        private static readonly Regex? _regexSplit = new Regex(@"(?<!\\)\/", RegexOptions.CultureInvariant | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        public const int MaxLenghtPaths = 10;
        public const int MaxTokenLen = 50;

        private readonly string[] _tokens;
        private readonly string _resource;

        public const string Star = "*";
        public const string NotSet = "*-";
        
        public PathString(string[] tokens, string resource = NotSet)
        {
            tokens = tokens.Where(w => w != PathString.NotSet).ToArray();
            resource = string.IsNullOrEmpty(resource) ? NotSet : resource;

            if (tokens.Length > PathString.MaxLenghtPaths)
                throw new ArgumentException($@"Max allowed tokens: {PathString.MaxLenghtPaths}", nameof(tokens));

            if (tokens.Length == 0 && resource == NotSet)
                throw new ArgumentException($@"At least 1 token is needed", nameof(tokens));

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Length > MaxTokenLen)
                    throw new ArgumentException($@"Tokens should be less than {MaxTokenLen} characters in length", nameof(tokens));
            }

            _tokens = tokens;
            _resource = resource;
        }

        public static PathString Parse(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("path should not be null or a whitespace", nameof(path));

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            if (!path.StartsWith('/'))
                throw new ArgumentException(@"path should start with '/' character", nameof(path));
#else
            if (!path.StartsWith("/", StringComparison.Ordinal))
                throw new ArgumentException(@"path should start with '/' character", nameof(path));
#endif

            var tokens = _regexSplit.Split(path.Substring(1))
                .Select(s => s.Replace(@"\/", @"/"))
                .ToArray();


#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            if (path.EndsWith('/') && !path.EndsWith(@"\/", StringComparison.Ordinal))
                return new PathString(tokens);
#else
            if (path.EndsWith("/", StringComparison.Ordinal) && !path.EndsWith(@"\/", StringComparison.Ordinal))
                return new PathString(tokens);
#endif


            return new PathString(tokens.Take(tokens.Length-1).ToArray(), tokens[tokens.Length-1]);
        }

        public static bool TryParse(string path, out PathString retVal)
        {
            try
            {
                retVal = PathString.Parse(path);

                return true;
            }
            catch (Exception)
            {
                retVal = null;

                return false;
            }
        }

        public bool IsRoot()
        {
            return _tokens.Length == 1 && _tokens[0] == "";
        }

        public bool IsResource()
        {
            return _resource != NotSet;
        }

        public bool IsMatch()
        {
            return _tokens.Any(x => x.EndsWith(Star, StringComparison.Ordinal)) || _resource.EndsWith(Star, StringComparison.Ordinal);
        }

        public string GetPath()
        {
            return ToString();
        }
        
        public string GetToken(int i)
        {
            if (i >= 0 && i < _tokens.Length)
            {
                return _tokens[i];
            }
            return PathString.NotSet;
        }

        public string GetResource()
        {
            return _resource;
        }

        public override string ToString()
        {
            return $"/{string.Join("/", _tokens.Select(x => x.Replace("/", @"\/")))}{(this.IsResource() ? $"/{GetResource().Replace("/", @"\/")}" : string.Empty)}";
        }

        public static implicit operator string(PathString url) { return url.ToString(); }

        public static implicit operator PathString(string url) { return PathString.Parse(url); }

        public static bool operator ==(PathString obj1, PathString obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(PathString obj1, PathString obj2)
        {
            return !obj1.Equals(obj2);
        }

        public bool Equals(PathString other)
        {
            return Enumerable.SequenceEqual(_tokens, other._tokens, StringComparer.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is PathString p && this.Equals(p);
        }

        public override int GetHashCode()
        {
            var hashCode = 2064342430;
            hashCode = hashCode * -1521134295 + StringComparer.Ordinal.GetHashCode(ToString());
            return hashCode;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    internal static class Ex
    {
        public static IEnumerable<object> ToPathDbParameter(this IEnumerable<PathString> pathStrings)
        {
            return pathStrings.Select((s, i) => new{
                PathId = i,
                PathName = s.ToString(),
                Level0 = s.GetToken(0),
                Level1 = s.GetToken(1),
                Level2 = s.GetToken(2),
                Level3 = s.GetToken(3),
                Level4 = s.GetToken(4),
                Level5 = s.GetToken(5),
                Level6 = s.GetToken(6),
                Level7 = s.GetToken(7),
                Level8 = s.GetToken(8),
                Level9 = s.GetToken(9),
                FileName = s.GetResource(),
            });
        }
    }

    /// <summary>
    /// Helpers for delimited string, with support for escaping the delimiter
    /// character.
    /// </summary>
    public static class DelimitedString
    {
        private const string _delimiterString = "/";
        private const char _delimiterChar = '/';

        // Use a single / as an escape character, avoid \ as that would require
        // all escape characters to be escaped in the source code...
        private const char _escapeChar = '\\';
        private const string _escapeString = "\\";

        /// <summary>
        /// Join strings with a delimiter and escape any occurence of the
        /// delimiter and the escape character in the string.
        /// </summary>
        /// <param name="strings">Strings to join</param>
        /// <returns>Joined string</returns>
        public static string Join(params string[] strings)
        {
            return string.Join(
              _delimiterString,
              strings.Select(
                s => s
                .Replace(_escapeString, _escapeString + _escapeString)
                .Replace(_delimiterString, _escapeString + _delimiterString)));
        }

        /// <summary>
        /// Split strings delimited strings, respecting if the delimiter
        /// characters is escaped.
        /// </summary>
        /// <param name="source">Joined string from <see cref="Join(string[])"/></param>
        /// <returns>Unescaped, split strings</returns>
        public static string[] Split(string source)
        {
            if (source.Length == 0)
                return new[] { "" };

            var result = new List<string>();

            int segmentStart = 0;
            for (int i = 0; i < source.Length; i++)
            {
                bool readEscapeChar = false;
                if (source[i] == _escapeChar)
                {
                    readEscapeChar = true;
                    i++;
                }

                if (!readEscapeChar && source[i] == _delimiterChar)
                {
                    result.Add(_unEscapeString(
                      source.Substring(segmentStart, i - segmentStart)));
                    segmentStart = i + 1;
                }

                if (i == source.Length - 1)
                {
                    result.Add(_unEscapeString(source.Substring(segmentStart)));
                }
            }

            return result.ToArray();
        }

        static string _unEscapeString(string src)
        {
            return src.Replace(_escapeString + _delimiterString, _delimiterString)
              .Replace(_escapeString + _escapeString, _escapeString);
        }
    }
}
