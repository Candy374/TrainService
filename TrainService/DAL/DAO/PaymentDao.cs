using Arch.Data.DbEngine;
using DAL.Entity;
using LoggerContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CommonUtilities;

namespace DAL.DAO
{
    public class PaymentDao
    {
        internal readonly BaseDaoWithLogger _baseDao = new BaseDaoWithLogger("userdb");

        public bool AddPayment(uint orderId, decimal amount, string tradeNumber)
        {
            var payment = GetPayment(orderId);
            if (payment != null)
            {
                Logger.Error("Already has a payment record for order id={0}. payment id={1}, log={2}".FormatedWith(orderId, payment.Id, payment.Log), "PaymentDao");
                return false;
            }

            var p = new PaymentEntity
            {
                OrderId = orderId,
                Amount = amount,
                PayTime = DateTime.Now,
                TradeNumber = tradeNumber,
                Log = "Pay {0} at {1}".FormatedWith(amount, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var id = _baseDao.Insert(p);

            return Convert.ToInt32(id) > 0;
        }

        public IList<PaymentEntity> GetNeedRefundPaymentList()
        {
            var sql = "SELECT * FROM payment WHERE need_refund>0 AND refund_trade_number is null";
            return _baseDao.SelectList<PaymentEntity>(sql);
        }

        public void SetRefundTradeNumber(uint paymentId, string tradeNo)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@PID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = paymentId });
            para.Add(new StatementParameter { Name = "@TradeNo", Direction = ParameterDirection.Input, DbType = DbType.String, Value = tradeNo });
            var sql = "UPDATE payment SET refund_trade_number=@TradeNo WHERE id=@PID AND refund_trade_number is null Limit 1";
            _baseDao.ExecNonQuery(sql, para);
        }

        public void SetRefundFee(uint paymentId, decimal fee,string tradeNo)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@PID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = paymentId });
            para.Add(new StatementParameter { Name = "@FEE", Direction = ParameterDirection.Input, DbType = DbType.Decimal, Value = fee });
            para.Add(new StatementParameter { Name = "@TradeNo", Direction = ParameterDirection.Input, DbType = DbType.String, Value = tradeNo });
            var sql = "UPDATE payment SET refund=@FEE WHERE id=@PID AND refund_trade_number=@TradeNo Limit 1";
            _baseDao.ExecNonQuery(sql, para);
        }

        public IList<PaymentEntity> GetRefundCheckList()
        {
            var sql = "SELECT * FROM payment WHERE need_refund>0 AND refund=0 AND refund_trade_number is not null";
            return _baseDao.SelectList<PaymentEntity>(sql);
        }

        public bool AddRefund(uint orderId, decimal refundAmount, string refundTradeNumber)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为30秒
            transactionOption.Timeout = new TimeSpan(0, 0, 30);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                var payment = GetPayment(orderId);
                if (payment == null)
                {
                    Logger.Error("No payment record found for order id={0}.".FormatedWith(orderId), "PaymentDao");
                    return false;
                }

                if (payment.RefundAmount > 0 || !string.IsNullOrEmpty(payment.RefundTradeNumber))
                {
                    Logger.Error("Already has refund recored in payment, order id=" + orderId, "PaymentDao");
                }

                payment.RefundAmount = refundAmount;
                payment.RefundTradeNumber = refundTradeNumber;
                payment.Log = "Refund {0} at {1}".FormatedWith(refundAmount, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var affectedRowCount = _baseDao.Update(payment);
                if (affectedRowCount != 1)
                {
                    Logger.Warn("Add refund record affect {0} rows. Order id={1}".FormatedWith(affectedRowCount, orderId), "PaymentDao");
                }
                ts.Complete();

                return affectedRowCount > 0;
            }
        }

        public PaymentEntity GetPayment(uint orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            var sql = "SELECT * FROM payment WHERE order_id=@OId Limit 1";

            return _baseDao.SelectList<PaymentEntity>(sql, para).FirstOrDefault();
        }

        public void SetRefundAmount(uint orderId, decimal userPayFee)
        {
            var pay = GetPayment(orderId);
            if (pay == null)
            {
                Logger.Error("Could not found payment, order id=" + orderId, "SetRefundAmount");
                throw new Exception("set refund failed!");
            }

            if (pay.Amount != userPayFee)
            {
                Logger.Error("pay.Amount({0})!=userPayFee({1})".FormatedWith(pay.Amount, userPayFee), "SetRefundAmount");
                throw new Exception("set refund failed!");
            }

            if (pay.NeedRefund > 0 && pay.NeedRefund != userPayFee)
            {
                Logger.Error("pay.RefundAmount({0})!=userPayFee({1})".FormatedWith(pay.NeedRefund, userPayFee), "SetRefundAmount");
                throw new Exception("set refund failed!");
            }

            if (pay.NeedRefund == userPayFee)
            {
                return;
            }

            pay.Log = "Set to need refund {0} at {1}".FormatedWith(userPayFee, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            pay.NeedRefund = userPayFee;

            _baseDao.Update(pay);
        }
    }
}
