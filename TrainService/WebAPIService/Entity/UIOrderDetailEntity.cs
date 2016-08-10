using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class UIOrderDetailEntity
    {
        [DataMember]
        public int OrderId { get; set; }

        public string OrderStatus { get; set; }
        public string Contact { get; set; }
        public string ContactTel { get; set; }
        public string OrderDetail { get; set; }
        public decimal Amount { get; set; }
        public string TrainNumber { get; set; }
        public 
    }
}