using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPIService.Entity
{
    [Serializable]
    [DataContract]
    public class UIMyOrdersEntity
    {
        public UIMyOrdersEntity()
        {
            OrderStatusEnum = new List<string>();
            OrderTypesEnum = new List<string>();
            Orders = new List<UIMyOrderEntity>();
        }


        [DataMember]
        public List<string> OrderTypesEnum { get; set; }

        [DataMember]
        public List<string> OrderStatusEnum { get; set; }

        [DataMember]
        public List<UIMyOrderEntity> Orders { get; set; }
    }
}