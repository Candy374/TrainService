using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class UIGoodsEntity
    {
        /// <summary>
        /// provider's ID
        /// </summary>
        [DataMember]
        public UInt32 GoodsId { get; set; }
        /// <summary>
        /// Goods' name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal SellPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool CanChangeFlavor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string PictureUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int OrderCount { get; set; }
       
        [DataMember]
        public int[] Tags { get; set; }
    }
}