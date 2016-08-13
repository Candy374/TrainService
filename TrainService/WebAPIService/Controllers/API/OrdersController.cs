using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DAL.Entity;
using DAL;
using System.Text;
using WebAPIService.Entity;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Route("Add")]
        public int Add([FromBody]dynamic data)
        {
            string openId = data.OpenId;
            string trainNumber = data.TrainNumber;
            string carriageNumber = data.CarriageNumber;
            bool isDelay = data.IsDelay;
            int orderType = data.OrderType;
            int payWay = data.PayWay;
            int manCount = data.ManCount;
            string comment = data.Comment;
            string contact = data.Contact;
            string contactTel = data.ContactTel;
            IEnumerable<dynamic> goodsList = data.List;
            decimal totalPriceFromUI = data.TotalPrice;
            decimal totalPriceVerify = 0;
            var orderDetailList = new List<OrderDetailEntity>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in goodsList)
            {
                var goods = DAL.DalFactory.Goods.GetGoods((uint)item.Id);
                if (goods == null)
                {
                    return -1;
                }

                orderDetailList.Add(new OrderDetailEntity
                {
                    GoodsId = (uint)item.Id,
                    ProviderId = goods.ProviderId,
                    PurchasePrice = goods.PurchasePrice,
                    SellPrice = goods.SellPrice,
                    Count = item.Count,
                    RefundCount = 0,
                    DisplayName = goods.Name
                });

                var price = goods.SellPrice * (int)item.Count;
                sb.AppendLine(goods.Name + "×" + item.Count + "=" + price);
                totalPriceVerify += price;
            }

            if (totalPriceFromUI != totalPriceVerify)
            {
                return -2;
            }

            var orderId = DalFactory.Orders.AddOrder(new OrderEntity
            {
                Amount = totalPriceVerify,
                CarriageNumber = carriageNumber,
                ContactNumber = contactTel,
                Contacts = contact,
                IsDelay = isDelay,
                OrderCreateTime = DateTime.Now,
                OrderDate = DateTime.Today,
                OrderDetail = sb.ToString(),
                OrderMsg = comment,
                OrderStatus = 0,
                OrderType = orderType,
                PayWay = payWay,
                TrainNumber = trainNumber,
                UserOpenid = openId,
                UserPayFee = 0,
                Refund = 0,
                ManCount = manCount
            }, orderDetailList);

            return Convert.ToInt32(orderId);
        }

        [Route("Query/{openId}")]
        public Entity.UIMyOrdersEntity Get(string openId)
        {
            var list = DalFactory.Orders.GetOrderByOpenId(openId);
            var ret = new Entity.UIMyOrdersEntity();
            foreach (var item in list)
            {
                var orderStatus = item.GetOrderStatusDisplayName();
                if (!ret.OrderStatusEnum.Contains(orderStatus))
                {
                    ret.OrderStatusEnum.Add(orderStatus);
                }

                var orderType = item.GetOrderTypeDisplayName();
                if (!ret.OrderTypesEnum.Contains(orderType))
                {
                    ret.OrderTypesEnum.Add(orderType);
                }

                var order = new Entity.UIMyOrderEntity
                {
                    Amount = item.Amount,
                    OrderId = item.OrderId,
                    OrderStatus = orderStatus,
                    OrderTime = item.OrderCreateTime,
                    OrderDate = item.OrderCreateTime.ToString("yyyy-MM-dd"),
                    OrderType = orderType,
                    TrainNumber = item.TrainNumber,
                    SubOrders = GetSubOrdersByOrderId(item.OrderId)
                };

                ret.Orders.Add(order);
            }

            return ret;
        }

        private List<UISubOrderEntity> GetSubOrdersByOrderId(uint orderId)
        {
            var list = DalFactory.Orders.GetSubOrdersSummary(Convert.ToInt32(orderId));
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
                    Count = count,
                    Name = item.DisplayName,
                    PicUrl = item.PicUrl,
                    Price = item.SellPrice * count
                });
            }

            return ret;
        }


    }
}
