using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class OrderStatus
    {
        public int StatusCode { get; set; }
        public string DisplayName { get; set; }

        public OrderStatus(string name, int code)
        {
            DisplayName = name;
            StatusCode = code;
        }
    }
}
