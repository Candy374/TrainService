﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPayApi
{
    public static class RefundHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction_id"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="total_fee">such as 10.50</param>
        /// <param name="refund_fee">such as 10.50</param>
        /// <returns></returns>
        public static string Refund(string transaction_id, string out_trade_no, decimal total_fee, decimal refund_fee)
        {
            Log.Info("Refund", "Refund is processing...");

            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(transaction_id))//微信订单号存在的条件下，则已微信订单号为准
            {
                data.SetValue("transaction_id", transaction_id);
            }
            else//微信订单号不存在，才根据商户订单号去退款
            {
                data.SetValue("out_trade_no", out_trade_no);
            }

            data.SetValue("total_fee", Convert.ToInt32(total_fee * 100));//订单总金额
            data.SetValue("refund_fee", Convert.ToInt32(refund_fee * 100));//退款金额
            data.SetValue("out_refund_no", WxPayApi.GenerateOutTradeNo());//随机生成商户退款单号
            data.SetValue("op_user_id", WxPayConfig.MCHID);//操作员，默认为商户号

            WxPayData result = WxPayApi.Refund(data);//提交退款申请给API，接收返回数据

            Log.Info("Refund", "Refund process complete, result : " + result.ToXml());

            if (result.IsSet("refund_id"))
            {
                return result.GetValue("refund_id").ToString();
            }

            return null;
        }

        /***
        * 退款查询完整业务流程逻辑
        * @param refund_id 微信退款单号（优先使用）
        * @param out_refund_no 商户退款单号
        * @param transaction_id 微信订单号
        * @param out_trade_no 商户订单号
        * @return 退款查询结果（xml格式）
        */
        public static decimal RefundQuery(string refund_id, string out_refund_no, string transaction_id, string out_trade_no)
        {
            Log.Info("RefundQuery", "RefundQuery is processing...");
            WxPayData data = new WxPayData();
            if (!string.IsNullOrEmpty(refund_id))
            {
                data.SetValue("refund_id", refund_id);//微信退款单号，优先级最高
            }
            else if (!string.IsNullOrEmpty(out_refund_no))
            {
                data.SetValue("out_refund_no", out_refund_no);//商户退款单号，优先级第二
            }
            else if (!string.IsNullOrEmpty(transaction_id))
            {
                data.SetValue("transaction_id", transaction_id);//微信订单号，优先级第三
            }
            else
            {
                data.SetValue("out_trade_no", out_trade_no);//商户订单号，优先级最低
            }

            WxPayData result = WxPayApi.RefundQuery(data);//提交退款查询给API，接收返回数据

            Log.Info("RefundQuery", "RefundQuery process complete, result : " + result.ToXml());

            if (result.IsSet("refund_fee_0"))
            {
                var fee = Convert.ToDecimal(result.GetValue("refund_fee_0").ToString());
                return fee / 100m;
            }

            return 0m;
        }
    }
}
