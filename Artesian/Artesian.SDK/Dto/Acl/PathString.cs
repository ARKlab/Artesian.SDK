﻿using System;
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
        private static Regex _regexSplit = new Regex(@"(?<!\\)\/");
        public const int MaxLenghtPaths = 10;
        public const int MaxTokenLen = 50;

        private string[] _tokens;
        private string _resource;

        public const string Star = "*";
        public const string NotSet = "*-";
        
        public PathString(string[] tokens, string resource = NotSet)
        {
            tokens = tokens.Where(w => w != PathString.NotSet).ToArray();
            resource = string.IsNullOrEmpty(resource) ? NotSet : resource;

            if (tokens.Length > PathString.MaxLenghtPaths)
                throw new ArgumentException($@"Max allowed tokens: {PathString.MaxLenghtPaths}");

            if (tokens.Length == 0 && resource == NotSet)
                throw new ArgumentException($@"At least 1 token is needed");

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Length > MaxTokenLen)
                    throw new ArgumentException($@"Tokens have to be less than {MaxTokenLen} chars lenght");
            }

            _tokens = tokens;
            _resource = resource;
        }

        public static PathString Parse(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException("path should not be null or whitespace");

            if (!path.StartsWith(@"/"))
                throw new ArgumentException(@"path should start with '/' character");

            var tokens = _regexSplit.Split(path.Substring(1))
                .Select(s => s.Replace(@"\/", @"/"))
                .ToArray();

            if (path.EndsWith("/") && !path.EndsWith(@"\/"))
                return new PathString(tokens);

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
            return _tokens.Any(x => x.EndsWith(Star)) || _resource.EndsWith(Star);
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
            return Enumerable.SequenceEqual(_tokens, other._tokens);
        }

        public override bool Equals(object obj)
        {
            return obj is PathString p && this.Equals(p);
        }

        public override int GetHashCode()
        {
            var hashCode = 2064342430;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ToString());
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
        const string DelimiterString = "/";
        const char DelimiterChar = '/';

        // Use a single / as escape char, avoid \ as that would require
        // all escape chars to be escaped in the source code...
        const char EscapeChar = '\\';
        const string EscapeString = "\\";

        /// <summary>
        /// Join strings with a delimiter and escape any occurence of the
        /// delimiter and the escape character in the string.
        /// </summary>
        /// <param name="strings">Strings to join</param>
        /// <returns>Joined string</returns>
        public static string Join(params string[] strings)
        {
            return string.Join(
              DelimiterString,
              strings.Select(
                s => s
                .Replace(EscapeString, EscapeString + EscapeString)
                .Replace(DelimiterString, EscapeString + DelimiterString)));
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
                if (source[i] == EscapeChar)
                {
                    readEscapeChar = true;
                    i++;
                }

                if (!readEscapeChar && source[i] == DelimiterChar)
                {
                    result.Add(UnEscapeString(
                      source.Substring(segmentStart, i - segmentStart)));
                    segmentStart = i + 1;
                }

                if (i == source.Length - 1)
                {
                    result.Add(UnEscapeString(source.Substring(segmentStart)));
                }
            }

            return result.ToArray();
        }

        static string UnEscapeString(string src)
        {
            return src.Replace(EscapeString + DelimiterString, DelimiterString)
              .Replace(EscapeString + EscapeString, EscapeString);
        }
    }
}
