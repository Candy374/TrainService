using Arch.Data.DbEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    static class Extensions
    {
        public static string ToString2(this StatementParameterCollection parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in parameters)
            {
                sb.AppendFormat("[{0}]\t[{1}]\t{2}={3}", item.Direction.ToString(), item.DbType.ToString(), item.Name, item.Value.ToString());
            }

            return sb.ToString();
        }
    }
}
