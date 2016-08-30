using LoggerContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAPIService.Controllers
{
    public class PayNotifyController : Controller
    {
        // GET: PayNotify
        [HttpPost]
        public string Index()
        {
            if (Request.HttpMethod.ToLower() == "post")
            {
                try
                {
                    var builder = new StringBuilder();
                    using (System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream)
                    {
                        int count = 0;
                        byte[] buffer = new byte[1024];
                        while ((count = s.Read(buffer, 0, 1024)) > 0)
                        {
                            builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                        }
                        s.Flush();
                    }

                    Logger.Info("Receive data from WeChat : " + builder.ToString());

                    var notify = new WxPayApi.ResultNotify(builder.ToString());

                    int payFee;
                    string tradeNo, orderIdStr;
                    var result = notify.ProcessNotify(out payFee, out orderIdStr, out tradeNo);

                    if (result.IsSet("return_code") && result.GetValue("return_code").ToString() == "SUCCESS")
                    {
                        uint orderId;
                        if (!uint.TryParse(orderIdStr, out orderId))
                        {
                            Logger.Critical("Wrong format of order id. Order id = " + orderIdStr);
                            result.SetValue("return_code", "FAIL");
                            result.SetValue("return_msg", "Wrong format of order id. Order id = " + orderIdStr);

                            return result.ToXml();
                        }

                        var isUpdateOrderSuccess = DAL.DalFactory.Orders.UpdatePayResult(orderId, Convert.ToDecimal(payFee) / Convert.ToDecimal(100), tradeNo);
                        if (!isUpdateOrderSuccess)
                        {
                            Logger.Critical("Receive pay result from WX, but update order failed!");
                            result.SetValue("return_code", "FAIL");
                            result.SetValue("return_msg", "Receive pay result from WX, but update order failed!");

                            return result.ToXml();
                        }

                    } // if (result.IsSet("return_code") && result.GetValue("return_code").ToString() == "SUCCESS")

                    return result.ToXml();
                }
                catch (Exception ex)
                {
                    Logger.Critical(ex, "Receive pay result but error occured.");
                    return string.Empty;
                }


            } //if (Request.HttpMethod.ToLower() == "post")

            return string.Empty;
        }
    }
}