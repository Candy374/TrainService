using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class AddOrderResult
    {
        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
        [DataMember]
        public uint OrderId { get; set; }
        [DataMember]
        public long TimeStamp { get; set; }
        [DataMember]
        public string NonceStr { get; set; }
        [DataMember]
        public string Package { get; set; }
        [DataMember]
        public string SignType { get; set; }
        [DataMember]
        public string PaySign { get; set; }
        [DataMember]
        public string Detail { get; set; }
        [DataMember]
        public decimal TotalFee { get; set; }
        [DataMember]
        public string OpenId { get; set; }
    }
}