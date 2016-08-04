using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class ConvertExtentions
    {
        public static int ToInt(this string numberStr)
        {
            int i;
            if (int.TryParse(numberStr, NumberStyles.None, new NumberFormatInfo(), out i))
            {
                return i;
            }

            if (int.TryParse(numberStr, NumberStyles.HexNumber, new NumberFormatInfo(), out i))
            {
                return i;
            }

            if (int.TryParse(numberStr, NumberStyles.Integer, new NumberFormatInfo(), out i))
            {
                return i;
            }

            if (int.TryParse(numberStr, NumberStyles.Float, new NumberFormatInfo(), out i))
            {
                return i;
            }

            if (int.TryParse(numberStr, NumberStyles.Number, new NumberFormatInfo(), out i))
            {
                return i;
            }

            if (int.TryParse(numberStr, NumberStyles.Currency, new NumberFormatInfo(), out i))
            {
                return i;
            }


            if (int.TryParse(numberStr, NumberStyles.Any, new NumberFormatInfo(), out i))
            {
                return i;
            }


            return 0;
        }

        public static bool ToBool(this string boolStr)
        {
            if (string.IsNullOrEmpty(boolStr))
            {
                return false;
            }

            var s = boolStr.ToUpper();
            switch (s)
            {
                case "T":
                case "TRUE":
                case "Y":
                case "YES":
                case "OK":
                    return true;
                default:
                    return false;
            }
        }

        public static DateTime ToDateTime(this string dateTimeStr, string format = null)
        {
            DateTime dateTime;
            if (string.IsNullOrEmpty(format))
            {
                if (DateTime.TryParse(dateTimeStr, out dateTime))
                {
                    return dateTime;
                }

                throw new Exception<ConvertException>(
                    string.Format("Can't convert {0}:{1} to {2}",
                    "string", dateTimeStr, "DateTime"));
            }
            else
            {
                if (DateTime.TryParseExact(dateTimeStr, format, new CultureInfo("en-US"),
                           DateTimeStyles.None, out dateTime))
                {
                    return dateTime;
                }

                throw new Exception<ConvertException>(
                    string.Format("Can't convert {0}:{1} to {2}",
                    "string", dateTimeStr, "DateTime"));
            }
        }

        public static TimeSpan ToTimeSpan(this string durationValue)
        {
            long duration;
            if (!long.TryParse(durationValue, out duration))
            {
                TimeSpan timeSpan;

                if (TimeSpan.TryParse(durationValue, out timeSpan))
                {
                    return timeSpan;
                }
                else
                {
                    throw new Exception<ConvertException>(
                        string.Format("Can't convert {0}:{1} to {2}",
                        "string", durationValue, "TimeSpan"));
                }
            }
            else
            {
                return new TimeSpan(duration);
            }

        }
    }
}
