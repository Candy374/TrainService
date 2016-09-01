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
using WxPayApi;

namespace WebAPIService.Controllers.API
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Pay")]
    public class PayController : ApiController
    {
        [Route("Order/{orderId}/IP/{ip}")]
        public string PrePay(uint orderId, string ip)
        {
            ip = ip.Replace('_', '.');
            var order = DalFactory.Orders.GetOrderByOrderId(orderId);
            if (order == null)
            {
                return "Err:找不到指定订单";
            }

            if (order.UserOpenid.Length < 28)
            {
                return "Err:无效的OpenId";

            }

            var payInfo = new WxJsPayInfo
            {
                Body = "河南宏之途商贸有限公司-外卖",
                ClientIp = ip,
                Openid = order.UserOpenid,
                Out_trade_no = order.OrderId.ToString(),
                Time_expire_span = new TimeSpan(0, 15, 0),
                Total_fee = Convert.ToInt32(order.Amount * 100).ToString()
            };
            var payApi = new JsApiPay(payInfo);

            if (string.IsNullOrEmpty(order.PrePayId))
            {
                DateTime expiredTime;
                var result = payApi.GetUnifiedOrderResult(out expiredTime);
                var prePayId = result.GetValue("prepay_id").ToString();
                var isAddPrePayInfoSuccess = DalFactory.Orders.UpdatePrePayInfo(order.OrderId, expiredTime, prePayId, order.LastChangeTime);
                if (!isAddPrePayInfoSuccess)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (DalFactory.Orders.UpdatePrePayInfo(order.OrderId, expiredTime, prePayId, order.LastChangeTime))
                        {
                            break;
                        }
                    }
                }

                return payApi.GetJsApiParameters();
            }
            else
            {
                var prePayId = order.PrePayId;

                return payApi.GetJsApiParameters(prePayId);
            }

        }
        

        [HttpGet]
        [Route("Test/{orderId}/IP/{ip}")]
        public string test(string orderId, string ip)
        {
            return orderId + ip;
        }
    }
}
