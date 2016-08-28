using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net;
using System.Web.Security;
using LitJson;

namespace WxPayApi
{
    public class JsApiPay
    {
        /// <summary>
        /// 保存页面对象，因为要在类的方法中使用Page的Request对象
        /// </summary>
        private WxJsPayInfo _payInfo { get; set; }

        /// <summary>
        /// openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// access_token用于获取收货地址js函数入口参数
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        public JsApiPay(WxJsPayInfo payInfo)
        {
            _payInfo = payInfo;
        }


        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayData GetUnifiedOrderResult(out DateTime expiredTime)
        {
            var now = DateTime.Now;
            expiredTime = now.Add(_payInfo.Time_expire_span);

            //统一下单
            WxPayData data = new WxPayData();
            data.SetValue("body", _payInfo.Body);
            data.SetValue("out_trade_no", _payInfo.Out_trade_no);
            data.SetValue("total_fee", _payInfo.Total_fee);
            data.SetValue("time_start", now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", expiredTime.ToString("yyyyMMddHHmmss"));
            data.SetValue("fee_type", "CNY");
            data.SetValue("spbill_create_ip", _payInfo.ClientIp);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("notify_url", WxPayConfig.NOTIFY_URL);
            data.SetValue("openid", _payInfo.Openid);

            WxPayData result = WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            unifiedOrderResult = result;
            return result;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public object GetJsApiParameters(string prePayId = null)
        {
            Log.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", WxPayConfig.APPID);
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + prePayId ?? unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            Log.Debug(this.GetType().ToString(), "Get jsApiParam : " + jsApiParam.ToJson());
            var parameters = jsApiParam.ToObj();

            return parameters;
        }


        /**
	    * 
	    * 获取收货地址js函数入口参数,详情请参考收货地址共享接口：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_9
	    * @return string 共享收货地址js函数需要的参数，json格式可以直接做参数使用
	    */
        //public string GetEditAddressParameters()
        //{
        //    string parameter = "";
        //    try
        //    {
        //        string host = page.Request.Url.Host;
        //        string path = page.Request.Path;
        //        string queryString = page.Request.Url.Query;
        //        //这个地方要注意，参与签名的是网页授权获取用户信息时微信后台回传的完整url
        //        string url = "http://" + host + path + queryString;

        //        //构造需要用SHA1算法加密的数据
        //        WxPayData signData = new WxPayData();
        //        signData.SetValue("appid", WxPayConfig.APPID);
        //        signData.SetValue("url", url);
        //        signData.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
        //        signData.SetValue("noncestr", WxPayApi.GenerateNonceStr());
        //        signData.SetValue("accesstoken", access_token);
        //        string param = signData.ToUrl();

        //        Log.Debug(this.GetType().ToString(), "SHA1 encrypt param : " + param);
        //        //SHA1加密
        //        string addrSign = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");
        //        Log.Debug(this.GetType().ToString(), "SHA1 encrypt result : " + addrSign);

        //        //获取收货地址js函数入口参数
        //        WxPayData afterData = new WxPayData();
        //        afterData.SetValue("appId", WxPayConfig.APPID);
        //        afterData.SetValue("scope", "jsapi_address");
        //        afterData.SetValue("signType", "sha1");
        //        afterData.SetValue("addrSign", addrSign);
        //        afterData.SetValue("timeStamp", signData.GetValue("timestamp"));
        //        afterData.SetValue("nonceStr", signData.GetValue("noncestr"));

        //        //转为json格式
        //        parameter = afterData.ToJson();
        //        Log.Debug(this.GetType().ToString(), "Get EditAddressParam : " + parameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(this.GetType().ToString(), ex.ToString());
        //        throw new WxPayException(ex.ToString());
        //    }

        //    return parameter;
        //}
    }

    public class WxJsPayInfo
    {
        public string Body { get; set; }
        public string Out_trade_no { get; set; }
        public string Total_fee { get; set; }
        public string ClientIp { get; set; }
        public TimeSpan Time_expire_span { get; set; }
        public string Openid { get; set; }
    }
}