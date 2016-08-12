using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class UIMyOrderEntity
    {
        [DataMember]
        public string OrderType { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime OrderTime { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public uint OrderId { get; set; }
    }
}