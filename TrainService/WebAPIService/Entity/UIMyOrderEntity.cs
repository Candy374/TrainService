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
        public UIMyOrderEntity() { }

        public UIMyOrderEntity(DAL.Entity.OrderEntity item)
        {
            var orderStatus = item.GetOrderStatusDisplayName();
            var orderType = item.GetOrderTypeDisplayName();
            Amount = item.Amount;
            OrderId = item.OrderId;
            OrderStatus = orderStatus;
            StatusCode = item.OrderStatus;
            OrderTime = item.OrderCreateTime;
            OrderDate = item.OrderCreateTime.ToString("yyyy-MM-dd");
            OrderType = orderType;
            TrainNumber = item.TrainNumber;
            Contact = item.Contacts;
            ContactTel = item.ContactNumber;
            CarriageNumber = item.CarriageNumber;
            Comment = item.OrderMsg;
            IsDelay = item.IsDelay;
            SubOrders = GetSubOrdersByOrderId(item.OrderId);
            IsRated = item.IsRated;
            ExpiredTime = item.ExpiredTime ?? DateTime.MinValue;
        }

        private List<UISubOrderEntity> GetSubOrdersByOrderId(uint orderId)
        {
            var list = DAL.DalFactory.Orders.GetSubOrdersSummary(Convert.ToInt32(orderId));
            var ret = new List<UISubOrderEntity>();
            foreach (var item in list)
            {
                var count = item.Count - item.RefundCount;
                if (count <= 0)
                {
                    continue;
                }

                ret.Add(new UISubOrderEntity
                {
                    Id = item.Id,
                    Count = count,
                    Name = item.DisplayName,
                    PicUrl = item.PicUrl,
                    Price = item.SellPrice * count,
                    GoodsId = item.GoodsId,
                    Rate = item.Rate,
                    ProviderId = item.ProviderId,
                    ProviderName = DAL.DalFactory.Provider.Search(Convert.ToUInt32(item.ProviderId)).Name
                });
            }

            return ret;
        }

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
        [DataMember]
        public List<UISubOrderEntity> SubOrders { get; set; }


        [DataMember]
        public string OrderDate { get; set; }
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public string TrainNumber { get; set; }
        [DataMember]
        public string Contact { get; set; }
        [DataMember]
        public string ContactTel { get; set; }
        [DataMember]
        public bool IsDelay { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string CarriageNumber { get; set; }
        [DataMember]
        public bool IsRated { get; set; }
        [DataMember]
        public DateTime ExpiredTime { get; set; }

    }
}