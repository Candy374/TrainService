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
using CommonUtilities;
using DAL.DAO;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {


        [Route("Add")]
        public int Add([FromBody]dynamic data)
        {
            Logger.Info(Convert.ToString(data), "api/Orders/Add");
            ReturnData result = TryAddOrder(data);
            string openId = data.OpenId;

            if (result.Data != null && (int)result.Data > 0)
            {
                Logger.Info("Add Order success. order_id={0}, openId={1}".FormatedWith(result.Data, openId), "api/Orders/Add");

                return (int)result.Data;
            }
            else
            {
                Logger.Warn("Add Order FAIL. retuen_code={0}, openId={1}".FormatedWith(result.ErrCode, openId), "api/Orders/Add");
                throw new Exception(result.ErrMsg);
            }


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

        private static ReturnData TryAddOrder(dynamic data)
        {
            try
            {
                string openId = data.OpenId;
                if (string.IsNullOrEmpty(openId))
                {
                    return new ReturnData
                    {
                        Data = null,
                        ErrCode = 3,
                        ErrMsg = "OpenId is empry!",
                        ShowLevel = 1
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
                        return new ReturnData
                        {
                            Data = null,
                            ErrCode = 1,
                            ErrMsg = "Could not found good by goodsId"
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
                    return new ReturnData
                    {
                        Data = null,
                        ErrCode = 2,
                        ErrMsg = "Amount verification fail!",
                        ShowLevel = 1
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

                return new ReturnData
                {
                    Data = Convert.ToInt32(orderId),
                    ErrCode = 0,
                    ErrMsg = "",
                    ShowLevel = 0
                };

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "", "api/Orders/Add");
                return new ReturnData
                {
                    Data = null,
                    ErrCode = 4,
                    ErrMsg = ex.Message,
                    ShowLevel = 1
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


                //{  orderId : 123 , Rates: ["SubId":3, "GoodsId" : 1 , "Rate" : 5] , [ "goodsId" : 2 , "rate" : 3 ] }
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
                    //return new ReturnData
                    //{
                    //    Data = "FAIL",
                    //    ErrCode = 1,
                    //    ErrMsg = "Rate goods failed",
                    //    ShowLevel = 1
                    //};
                    throw new Exception("Rate goods failed");
                }

                return 1;
                //return new ReturnData
                //{
                //    Data = "SUCCESS",
                //    ErrCode = 0,
                //    ErrMsg = "",
                //    ShowLevel = 1
                //};
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "", "api/Orders/Rate");
                throw;
                //return new ReturnData
                //{
                //    Data = "FAIL",
                //    ErrCode = 1,
                //    ErrMsg = ex.Message,
                //    ShowLevel = 1
                //};
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
            if (string.IsNullOrEmpty(openId) || openId.ToUpper() == "TBD")
            {
                throw new ArgumentNullException("OpenId is " + (openId ?? "<null>"));
            }

            var order = DalFactory.Orders.GetOrderByOrderId(orderId);
            if (order == null)
            {
                throw new Exception("Could not find Order");
            }

            int oldStatus;
            var isCanceled = DalFactory.Orders.CancelOrder(openId, orderId, out oldStatus);
            if (isCanceled)
            {
                if (oldStatus == 1 || oldStatus == 2 || oldStatus == 3 || oldStatus == 4 || oldStatus == 5)
                {
                    DalFactory.Payment.SetRefundAmount(order.OrderId, order.UserPayFee);
                }
            }

            return 1;
        }

        [HttpPost]
        [Route("Delete/{openId}/{orderId}")]
        public int Delete(string openId, uint orderId)
        {
            Logger.Info("openId={0}, orderId={1}".FormatedWith(openId, orderId), "api/Orders/Delete/{openId}/{orderId}");
            return DalFactory.Orders.DeleteOrder(openId, orderId) ? 1 : 0;
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
        public UIMyOrderEntity GetByOrderId(uint orderId)
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

        [Route("Query/ProviderId/{provider}/Status/{status}")]
        public object GetSubOrdersByProviderId(string provider, int status)
        {
            IList<OrderDetailEntity> list;
            if (provider.ToUpper() == "ALL")
            {
                list = DalFactory.Orders.GetSubOrdersByStatus(status);
            }
            else
            {
                int providerId;
                if (!int.TryParse(provider, out providerId))
                {
                    throw new ArgumentException("provider is not id!");
                }

                list = DalFactory.Orders.GetSubOrdersByProviderId(providerId, status);
            }

            SortedDictionary<uint, List<OrderDetailEntity>> dic = new SortedDictionary<uint, List<OrderDetailEntity>>();
            var r = new List<dynamic>();

            foreach (var item in list)
            {
                if (!dic.ContainsKey(item.OrderId))
                {
                    dic.Add(item.OrderId, new List<OrderDetailEntity>());
                }

                dic[item.OrderId].Add(item);
            }

            foreach (var k in dic.Keys)
            {
                var d = new List<dynamic>();
                decimal amount = 0;
                foreach (var j in dic[k])
                {
                    var p = DalFactory.Provider.Search(Convert.ToUInt32(j.ProviderId));
                    d.Add(new
                    {
                        Id = j.ID,
                        GoodsId = j.GoodsId,
                        Name = j.DisplayName,
                        Count = j.Count,
                        PurchasePrice = j.PurchasePrice,
                        Provider = p
                    });
                    amount += j.PurchasePrice;
                }

                var o = DalFactory.Orders.GetOrderByOrderId(k);
                var train = DalFactory.TimeTable.Query("ZAF", o.TrainNumber);
                if (train == null)
                {
                    throw new Exception("Could not find train:" + o.TrainNumber);
                }

                r.Add(new
                {
                    OrderId = k,
                    Amount = amount,
                    TrainNumber = o.TrainNumber,
                    ExpectTime = StationsController.GetArriveTime(train),
                    SubOrders = d
                });
            }

            return r;
        }

        [Route("Update/SubOrder")]
        public int ChangeSubOrderStatus([FromBody]dynamic data)
        {
            int newStatus = data.NewStatus;
            int oldStatus = data.OldStatus;

            string openId = data.OpenId;

            List<uint> subOrderIds = new List<uint>();
            List<OrderDetailEntity> subOrders = new List<OrderDetailEntity>();
            foreach (uint id in data.SubOrderIds)
            {
                subOrderIds.Add(id);
            }

            // Auth verification
            foreach (var sid in subOrderIds)
            {
                var subOrder = DalFactory.Orders.GetSubOrderById(sid);
                subOrders.Add(subOrder);
                if (subOrder == null)
                {
                    throw new Exception("Invaild SubOrderID");
                }
                var openIds = DalFactory.Provider.GetOpenIdsByProviderId(subOrder.ProviderId);
                if (!openIds.Contains(openId))
                {
                    if (newStatus == 4)
                    {
                        var user = DalFactory.Account.CachedTable.Where(a => a.OpenId == openId).FirstOrDefault();
                        if (user != null)
                        {
                            if (user.GroupID == 103)
                            {
                                continue;
                            }
                        }
                    }
                    throw new Exception("You don't have permission to update the subOrder");
                }
            }

            var result = DalFactory.Orders.BulkChangeSubOrdersStatus(subOrderIds, (DAL.DAO.OrderStatus)newStatus, (DAL.DAO.OrderStatus)oldStatus);
            if (result)
            {
                foreach (var item in subOrders)
                {
                    OnSubOrderStatusChanged(item.ID, item.OrderId, (DAL.DAO.OrderStatus)oldStatus, (DAL.DAO.OrderStatus)newStatus);
                }
            }

            return result ? 1 : 0;
        }

        [Route("Update/Order/{orderId}")]
        public int ChangeOrderStatus(uint orderId, [FromBody]dynamic data)
        {
            int newStatus = data.NewStatus;
            int oldStatus = data.OldStatus;
            string openId = data.OpenId;
            if (string.IsNullOrEmpty(openId) || openId.ToUpper() == "TBD")
            {
                throw new ArgumentNullException("OpenId is " + (openId ?? "<null>"));
            }

            var order = DalFactory.Orders.GetOrderByOrderId(orderId);
            if (order == null)
            {
                throw new Exception("Could not find Order");
            }

            if (order.OrderStatus != oldStatus)
            {
                throw new Exception("OldStatus is changed, Please refresh data.");
            }

            var isStatusChanged = DalFactory.Orders.ChangeOrderStatus(
                orderId, (DAL.DAO.OrderStatus)newStatus, (DAL.DAO.OrderStatus)oldStatus);

            OnOrderStatusChanged(orderId, (DAL.DAO.OrderStatus)newStatus, (DAL.DAO.OrderStatus)oldStatus);

            return 1;

        }

        private void OnOrderStatusChanged(uint orderId, DAL.DAO.OrderStatus newStatus, DAL.DAO.OrderStatus oldStatus)
        {
            if (newStatus == DAL.DAO.OrderStatus.已支付
                || newStatus == DAL.DAO.OrderStatus.订单取消
                || newStatus == DAL.DAO.OrderStatus.已送到指定位置
                || newStatus == DAL.DAO.OrderStatus.订单结束)
            {
                DalFactory.Orders.ChangeSubOrderStatusByOrderId(orderId, newStatus, oldStatus);
            }
        }

        private void OnSubOrderStatusChanged(uint subOrderId, uint orderId, DAL.DAO.OrderStatus oldStatus, DAL.DAO.OrderStatus newStatus)
        {
            var order = DalFactory.Orders.GetOrderByOrderId(orderId);
            if (order.OrderStatus == (int)DAL.DAO.OrderStatus.已支付 && newStatus == DAL.DAO.OrderStatus.商家接单)
            {
                DalFactory.Orders.ChangeOrderStatus(orderId, DAL.DAO.OrderStatus.商家接单, DAL.DAO.OrderStatus.已支付);
                return;
            }

            if (order.OrderStatus == (int)DAL.DAO.OrderStatus.商家接单 && newStatus == DAL.DAO.OrderStatus.商家已配货)
            {
                var subOrders = DalFactory.Orders.GetSubOrders(Convert.ToInt32(orderId));
                var canChangeStatus = true;
                foreach (var item in subOrders)
                {
                    var s = (DAL.DAO.OrderStatus)item.Status;
                    if (s != DAL.DAO.OrderStatus.商家已配货 && s != DAL.DAO.OrderStatus.快递已取货 && s != DAL.DAO.OrderStatus.已送到指定位置)
                    {
                        canChangeStatus = false;
                        break;
                    }
                }

                if (canChangeStatus)
                {
                    DalFactory.Orders.ChangeOrderStatus(orderId, DAL.DAO.OrderStatus.商家已配货, DAL.DAO.OrderStatus.商家接单);
                }

                return;
            }

            if (order.OrderStatus == (int)DAL.DAO.OrderStatus.商家已配货 && newStatus == DAL.DAO.OrderStatus.快递已取货)
            {
                var subOrders = DalFactory.Orders.GetSubOrders(Convert.ToInt32(orderId));
                var canChangeStatus = true;
                foreach (var item in subOrders)
                {
                    var s = (DAL.DAO.OrderStatus)item.Status;
                    if (s != DAL.DAO.OrderStatus.快递已取货 && s != DAL.DAO.OrderStatus.已送到指定位置)
                    {
                        canChangeStatus = false;
                        break;
                    }
                }

                if (canChangeStatus)
                {
                    DalFactory.Orders.ChangeOrderStatus(orderId, DAL.DAO.OrderStatus.快递已取货, DAL.DAO.OrderStatus.商家已配货);
                }

                return;
            }
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
