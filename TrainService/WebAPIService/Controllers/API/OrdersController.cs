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
using LoggerContract;

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
            //int manCount = data.ManCount;
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
                ManCount = 0
            }, orderDetailList);

            return Convert.ToInt32(orderId);
        }


        [Route("Rate")]
        public int Rate([FromBody] dynamic rateInfo)
        {
            //{  orderId : 123 , rates: ["SubId":3, "goodsId" : 1 , "rate" : 5] , [ "goodsId" : 2 , "rate" : 3 ] }
            uint orderId = rateInfo.OrderId;
            Dictionary<uint, int> goodsRates = new Dictionary<uint, int>();
            Dictionary<uint, int> subOrderRates = new Dictionary<uint, int>();

            foreach (var item in rateInfo.Rates)
            {
                uint key = item.GoodsId;
                uint subId = item.SubId;
                goodsRates[key] = item.Rate;
                subOrderRates[subId] = item.Rate;

            }

            bool rated = DalFactory.Goods.Rate(goodsRates, subOrderRates);

            if (rated)
            {
                DalFactory.Orders.SetRated(orderId);
            }
            else
            {
                return 0;
            }

            return 1;
        }

        //[Route("Provider/{id}")]
        //public List<UISubOrderEntity> GetOrdersBuyProviderId(int id)
        //{
        //    var ret=new List<UISubOrderEntity>);
        //    var list = DalFactory.Orders.GetSubOrdersByProviderId(id);
        //    foreach (var item in list)
        //    {
        //        ret.Add(new UISubOrderEntity {
        //             Count=item.Count,
        //              Name=item.DisplayName,

        //        });
        //    }
        //}

        [Route("Query/All/{openId}")]
        public Entity.UIMyOrdersEntity Get(string openId)
        {
            var list = DalFactory.Orders.GetOrderByOpenId(openId);

            return BuildUIMyOrdersEntityFromOrderEntityList(list); ;
        }

        [Route("Cancel/{orderId}")]
        public int Cancel(uint orderId)
        {
            return DalFactory.Orders.CancelOrder(orderId) ? 1 : 0;
        }

        private UIMyOrdersEntity BuildUIMyOrdersEntityFromOrderEntityList(IList<OrderEntity> list)
        {
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

                var order = new Entity.UIMyOrderEntity(item);

                ret.Orders.Add(order);
            }

            return ret;
        }

        [Route("Query/Id/{openId}/Page/{pageNumber}/PageSize/{pageSize}")]
        public Entity.UIMyOrdersEntity Get(string openId, int pageNumber, int pageSize)
        {
            var list = DalFactory.Orders.GetOrderByOpenId(openId, pageSize, pageNumber);

            return BuildUIMyOrdersEntityFromOrderEntityList(list);
        }

        [Route("Query/Order/{orderId}")]
        public UIMyOrderEntity GetByOrderId(string orderId)
        {
            var item = DalFactory.Orders.GetOrderByOrderId(orderId);
            if (item == null)
            {
                return null;
            }

            var order = new UIMyOrderEntity(item);

            return order;
        }

        [Route("Update/SubOrder/{subOrderId}")]
        public int ChangeSubOrderStatus(uint subOrderId, [FromBody]dynamic data)
        {
            int newStatus = data.NewStatus;
            int oldStatus = data.OldStatus;
            var result = DalFactory.Orders.ChangeSubOrderStatus(subOrderId, (DAL.DAO.OrderStatus)newStatus, (DAL.DAO.OrderStatus)oldStatus);
            //if (result && newStatus == 2)
            //{
            //    OrderEntity order = DalFactory.Orders.GetOrderBySubOrderId(subOrderId);
            //}

            return result ? 1 : 0;
        }

   
    }
}
