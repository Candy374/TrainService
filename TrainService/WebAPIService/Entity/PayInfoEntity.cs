using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Controllers.API
{
    [DataContract]
    [Serializable]
    public class PayInfoEntity
    {
        [DataMember]
        public string appId { get; set; }
        [DataMember]
        public string timeStamp { get; set; }

        [DataMember]
        public string nonceStr { get; set; }
        [DataMember]
        public string package { get; set; }
        [DataMember]
        public string signType { get; set; }
        [DataMember]
        public string paySign { get; set; }
    }
}