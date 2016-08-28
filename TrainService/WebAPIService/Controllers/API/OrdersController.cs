﻿using System;
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
using CommonUtilities;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Route("Add")]
        public AddOrderResult Add([FromBody]dynamic data)
        {
            Logger.Info(Convert.ToString(data), "api/Orders/Add");
            AddOrderResult result = TryAddOrder(data);
            if (result.OpenId.Length >= 28)
            {
              //  PrePay(result);
            }
            return result;
        }

        //private void PrePay(AddOrderResult data)
        //{
        //    if (data.OrderId < 1)
        //    {
        //        return;
        //    }

        //    var timeStamp = DateTime.Now.ToUnixTimeStamp();
        //    var signType = "MD5";

        //    var paramDic = new Dictionary<string, string>();

        //    paramDic["appid"] = "wxaf1fff843c641aba";
        //    paramDic["nonce_str"] = Guid.NewGuid().ToString("N");
        //    paramDic["mch_id"] = "1380207502";// 商户号
        //    paramDic["device_info"] = "WEB";
        //    paramDic["body"] = "河南宏之途商贸有限公司-外卖";
        //    paramDic["out_trade_no"] = data.OrderId.ToString();
        //    paramDic["fee_type"] = "CNY";
        //    paramDic["total_fee"] = Convert.ToInt32(data.TotalFee * 100).ToString();
        //    paramDic["spbill_create_ip"] = "";//TODO: 获取网页端的IP
        //    paramDic["time_start"] = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    paramDic["time_expire"] = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");
        //    paramDic["notify_url"] = "";//支付回调Url
        //    paramDic["trade_type"] = "JSAPI";
        //    paramDic["openid"] = data.OpenId;
        //    string key = "AFN7SDFSADFH92FWUFN82F72NFNRF824"; //商户的Key
        //    var sign = Sign(paramDic, key);

        //}

        //private object Sign(Dictionary<string, string> paramDic, string key)
        //{
        //    var list = paramDic.Keys.ToList();
        //    list.Sort();
        //    var args = new List<string>();
        //    foreach (var k in list)
        //    {
        //        if (string.IsNullOrEmpty(paramDic[k]))
        //        {
        //            //空值不参与签名
        //            continue;
        //        }
        //        args.Add("{0}={1}".FormatedWith(k, paramDic[k]));
        //    }

        //    var tempStr = string.Join("&", args);
        //}

        private static AddOrderResult TryAddOrder(dynamic data)
        {
            try
            {
                string openId = data.OpenId;
                if (string.IsNullOrEmpty(openId))
                {
                    return new AddOrderResult
                    {
                        ErrorCode = 3,
                        ErrorMsg = "Open Id is empty!"
                    };
                }
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
                TryRecordLastInput(openId, contact, contactTel);
                foreach (var item in goodsList)
                {
                    var goods = DAL.DalFactory.Goods.GetGoods((uint)item.Id);
                    if (goods == null)
                    {
                        return new AddOrderResult
                        {
                            ErrorCode = 1,
                            ErrorMsg = "Invided goods Id"
                        };
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
                    return new AddOrderResult
                    {
                        ErrorCode = 2,
                        ErrorMsg = "Price is wrong!"
                    };
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

                return new AddOrderResult
                {
                    OrderId = orderId,
                    OpenId = openId,
                    Detail = sb.ToString(),
                    TotalFee = totalPriceVerify,
                    ErrorCode = 0,
                    ErrorMsg = ""
                };

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "", "api/Orders/Add");
                return new AddOrderResult
                {
                    ErrorCode = 4,
                    ErrorMsg = "Unkown error"
                };
            }
        }


        private static void TryRecordLastInput(string openId, string contact, string contactTel)
        {
            try
            {
                var user = DalFactory.Account.GetAccount(openId);
                if (user != null)
                {
                    user.LastContactName = contact;
                    user.LastContactTel = contactTel;
                    DalFactory.Account.Update(user);
                    Logger.Info("user last input is Updated, openId=" + openId, "TryRecordLastInput");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error when try to update user last input");
            }
        }

        [Route("Rate")]
        public int Rate([FromBody] dynamic rateInfo)
        {
            Logger.Info(rateInfo, "api/Orders/Rate");
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "", "api/Orders/Rate");
                return 0;
            }
        }

        [Route("Query/All/{openId}")]
        public Entity.UIMyOrdersEntity Get(string openId)
        {
            Logger.Info("openId=" + openId, "api/Orders/Query/All/{openId}");
            var list = DalFactory.Orders.GetOrderByOpenId(openId);
            Logger.Info("Get {0} items.".FormatedWith(list.Count), "api/Orders/Query/All/{openId}");
            return BuildUIMyOrdersEntityFromOrderEntityList(list); ;
        }

        [Route("Cancel/{openId}/{orderId}")]
        public int Cancel(string openId, uint orderId)
        {
            Logger.Info("openId={0}, orderId={1}".FormatedWith(openId, orderId), "api/Orders/Cancel/{openId}/{orderId}");
            return DalFactory.Orders.CancelOrder(openId, orderId) ? 1 : 0;
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
            Logger.Info("openId={0}, pageNumber={1}, pageSize={2}".FormatedWith(openId, pageNumber, pageSize), "api/Orders/Query/Id/{openId}/Page/{pageNumber}/PageSize/{pageSize}");
            var list = DalFactory.Orders.GetOrderByOpenId(openId, pageSize, pageNumber);

            return BuildUIMyOrdersEntityFromOrderEntityList(list);
        }

        [Route("Query/Order/{orderId}")]
        public UIMyOrderEntity GetByOrderId(string orderId)
        {
            Logger.Info("orderId={0}".FormatedWith(orderId), "api/Orders/Query/Order/{orderId}");
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

        [Route("Update/Order/{orderId}/OpenId/{openId}")]
        public int UpdateOpenId(uint orderId, string openId)
        {
            if (openId.Length < 28)
            {
                return 0;
            }

            return DalFactory.Orders.UpdateOpenId(orderId, "TBD", openId) ? 1 : 0;
        }

    }
}
