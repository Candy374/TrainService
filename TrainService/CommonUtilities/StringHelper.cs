using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class StringHelper
    {
        [Activity]
        public static string[] Split(this string source, string separator, bool removeEmptyEntries)
        {
            return source.Split(new[] { separator },
                removeEmptyEntries
                    ? StringSplitOptions.None
                    : StringSplitOptions.RemoveEmptyEntries);
        }

        public static string SafeStringFormat(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            if (args == null || args.Length == 0)
            {
                return format;
            }

            var reg = new Regex(@"\{\d{1,3}?\}|\{\{|\}\}", RegexOptions.Singleline);
            var sb = new StringBuilder();
            var lastIndex = 0;
            var mat = reg.Match(format);
            while (mat.Success)
            {
                switch (mat.Value)
                {
                    case "{{":
                        sb.Append(format.Substring(lastIndex, mat.Index - lastIndex)).Append('{');
                        break;
                    case "}}":
                        sb.Append(format.Substring(lastIndex, mat.Index - lastIndex)).Append('}');
                        break;
                    default:
                        var index = int.Parse(mat.Value.Trim('{', '}'));
                        if (args.Length > index)
                        {
                            var value = args[index] == null ? "<null>" : args[index].ToString();
                            sb.Append(format.Substring(lastIndex, mat.Index - lastIndex))
                              .Append(value);
                        }
                        else
                        {
                            sb.Append(mat.Value);
                        }
                        break;
                }
                lastIndex = mat.Index + mat.Length;
                mat = mat.NextMatch();
            }

            sb.Append(format.Substring(lastIndex));

            return sb.ToString();
        }

        public static string FormatedWith(this string format, params object[] args)
        {
            return SafeStringFormat(format, args);
        }

        public static bool StartsWithOneOf(this string source, IEnumerable<string> strs, bool ignoreCase = false)
        {
            // Ignore Case
            if (ignoreCase)
            {
                source = source.ToUpper();
                foreach (var s in strs)
                {
                    if (source.StartsWith(s.ToUpper()))
                    {
                        return true;
                    }
                }

                return false;
            }

            // Case sensitive
            foreach (var s in strs)
            {
                if (source.StartsWith(s))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Join<T>(this IEnumerable<T> source, string separator, Func<T, string> selector)
        {
            return string.Join(separator, source.Select((selector)));
        }

        public static string[] CutWithMaxLength(this string source, char separator, int maxLength, bool forceCut = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new string[] { };
            }

            var maxIndex = source.Length;

            if (maxIndex <= maxLength)
            {
                return new[] { source };
            }

            if (maxLength <= 0)
            {
                throw new ArgumentOutOfRangeException("maxLength must larger than 0!");
            }

            List<string> list = new List<string>();
            int startIndex = 0;
            int endIndex = Math.Min(source.Length - 1, maxLength);
            while (startIndex < endIndex && endIndex < source.Length)
            {
                if (endIndex == maxIndex || source[endIndex] == separator)
                {
                    list.Add(Cut(ref startIndex, ref endIndex, maxLength, source));
                }
                else
                {
                    if (forceCut)
                    {
                        list.Add(source.Substring(startIndex, endIndex - startIndex));
                        startIndex = endIndex;
                        endIndex = Math.Min(endIndex + maxLength, maxIndex);
                    }
                    else
                    {
                        bool isFoundPreviousSeparator = false;
                        for (int i = endIndex; i > startIndex; i--)
                        {
                            if (source[i] == separator)
                            {
                                isFoundPreviousSeparator = true;
                                endIndex = i;
                                list.Add(Cut(ref startIndex, ref endIndex, maxLength, source));
                            }
                        }

                        if (!isFoundPreviousSeparator)
                        {
                            for (; endIndex < maxIndex; endIndex++)
                            {
                                if (source[endIndex] == separator)
                                {
                                    isFoundPreviousSeparator = true;
                                    list.Add(Cut(ref startIndex, ref endIndex, maxLength, source));
                                }
                            }
                        }
                    }
                }
            }



            return list.ToArray();
        }

        private static string Cut(ref int startIndex, ref int endIndex, int maxLength, string source)
        {
            var s = source.Substring(startIndex, endIndex - startIndex);
            startIndex = endIndex + 1;
            endIndex = Math.Min(endIndex + maxLength + 1, source.Length);

            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="rule">when you use StrCompareRule.Regex,Contains, StartsWith and EndsWith are ignored</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string key, StrCompareRule rule)
        {
            if (rule.HasFlag(StrCompareRule.IgnoreCase))
            {
                source = source.ToUpper();
                key = key.ToUpper();
            }

            if (rule.HasFlag(StrCompareRule.Regex))
            {
                var reg = new Regex(key, RegexOptions.Singleline);
                var mat = reg.Match(source);
                return mat.Success;
            }
            else
            {
                if (rule.HasFlag(StrCompareRule.Contains))
                {
                    return source.Contains(key);
                }

                if (rule.HasFlag(StrCompareRule.StartsWith))
                {
                    return source.StartsWith(key);
                }

                if (rule.HasFlag(StrCompareRule.EndsWith))
                {
                    return source.EndsWith(key);
                }

                if (rule.HasFlag(StrCompareRule.Equals))
                {
                    return source.Equals(key);
                }
            }

            throw new NotImplementedException("This path should not be reached");
        }
    }

    public enum StrCompareRule
    {
        Regex = 2,
        StartsWith = 4,
        IgnoreCase = 8,
        EndsWith = 16,
        Contains = 32,
        Equals = 64
    }
}
