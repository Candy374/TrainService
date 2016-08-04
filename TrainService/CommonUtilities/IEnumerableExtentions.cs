using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class IEnumerableExtentions
    {
        public static List<T> RemoveDuplicate<T>(this IEnumerable<T> list)
        {
            return RemoveDuplicate(list, (a, b) => a.Equals(b));
        }

        public static List<T> RemoveDuplicate<T>(this IEnumerable<T> list,
            Func<T, T, bool> areEqualFunc)
        {
            var returnList = new List<T>();
            foreach (var item in list)
            {
                var existed = false;
                foreach (var exItem in returnList)
                {
                    if (areEqualFunc(item, exItem))
                    {
                        existed = true;
                        break;
                    }
                }

                if (!existed)
                {
                    returnList.Add(item);
                }
            }

            return returnList;
        }

        public static bool Contains<T>(this IEnumerable<T> list, IEnumerable<T> subList)
        {
            if (subList == null)
            {
                return true;
            }

            foreach (var a in subList)
            {
                var contains = false;
                foreach (var b in list)
                {
                    var areEquals = b.Equals(a);
                    if (areEquals)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains(this IEnumerable<string> list, IEnumerable<string> subList, bool ignoreCase)
        {
            if (subList == null)
            {
                return true;
            }


            foreach (var a in subList)
            {
                var contains = false;
                foreach (var b in list)
                {
                    var areEquals = b.Equals(a,
                        ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                    if (areEquals)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains(this IEnumerable<string> list, string str, bool ignoreCase)
        {
            var ignoreCaseOption = ignoreCase
                                       ? StringComparison.OrdinalIgnoreCase
                                       : StringComparison.Ordinal;
            return list.Any(b => b.Equals(str, ignoreCaseOption));
        }
    }
}
