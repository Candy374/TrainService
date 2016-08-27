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
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string ContactTel { get; set; }
        [DataMember]
        public string OrderDetail { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string TrainNumber { get; set; }
        [DataMember]
        public int GoodsId { get; set; }
    }
}