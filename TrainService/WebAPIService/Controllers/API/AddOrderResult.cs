using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Controllers
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
        public ulong TimeStamp { get; set; }
        [DataMember]
        public string NonceStr { get; set; }
        [DataMember]
        public string Package { get; set; }
        [DataMember]
        public string SignType { get; set; }
        [DataMember]
        public string PaySign { get; set; }
       
    }
}