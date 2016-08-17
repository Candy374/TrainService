using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class UISubOrderEntity
    {
        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public string PicUrl { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public uint GoodsId { get; set; }
        [DataMember]
        public int Rate { get; set; }
    }
}