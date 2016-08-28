using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class DateTimeHelper
    {
        public static long ToUnixTimeStamp(this DateTime dt)
        {
            var s = new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64((dt - s).TotalSeconds);
        }
    }
}
