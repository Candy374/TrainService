using Arch.Data;
using Arch.Data.DbEngine;
using CommonUtilities;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL.DAO
{
    public class OrderDao
    {
        internal readonly BaseDaoWithLogger _baseDao = new BaseDaoWithLogger("userdb");

        public uint AddOrder(OrderEntity entity, List<OrderDetailEntity> orderDetailList)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为30秒
            transactionOption.Timeout = new TimeSpan(0, 0, 30);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                var result = _baseDao.Insert(entity); ;
                var orderId = Convert.ToUInt32(result);
                for (int i = 0; i < orderDetailList.Count; i++)
                {
                    orderDetailList[i].OrderId = orderId;
                }
                _baseDao.BulkInsert(orderDetailList);
                ts.Complete();

                return orderId;
            }
        }

        public IList<OrderDetailEntity> GetSubOrders(int orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = orderId });

            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE order_id=@OId", para);
        }

        public IList<SubOrderSummaryEntity> GetSubOrdersSummary(int orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = orderId });

            return _baseDao.SelectList<SubOrderSummaryEntity>(
                "SELECT " +
                "order_details.id AS id, " +
                "goods.pic_url AS  url,  " +
                "order_details.buy_count AS buy_count,  " +
                "order_details.display_name AS display_name,  " +
                "order_details.sell_price AS sell_price, " +
                "order_details.refund_count AS refund_count, " +
                "order_details.goods_id AS goods_id, " +
                "order_details.rating AS rating, " +
                "order_details.provider_id AS provider_id " +
                "FROM order_details,goods  " +
                "WHERE  " +
                "order_details.order_id=@OId " +
                "AND " +
                "order_details.goods_id=goods.id", para);
        }

        public IList<OrderEntity> GetOrderByOpenId(string openId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = openId });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE user_openid=@OpenId AND order_status<>99 ORDER BY order_create_time DESC", para);
        }

        public IList<OrderEntity> GetOrderByOpenId(string openId, int pageSize, int pageNumber)
        {
            if (pageSize < 1 || pageNumber < 1)
            {
                return null;
            }

            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = openId });
            para.Add(new StatementParameter { Name = "@StartIndex", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = pageSize * (pageNumber - 1) });
            para.Add(new StatementParameter { Name = "@PageSize", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = pageSize });
            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders  WHERE user_openid=@OpenId  AND order_status<>99 ORDER BY order_create_time DESC LIMIT @StartIndex,@PageSize", para);
        }

        public void SetRated(uint orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            _baseDao.ExecNonQuery("UPDATE orders SET is_rated=TRUE WHERE order_id=@OID", para);
        }

        public OrderEntity GetOrderByOrderId(uint orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_id=@OId  AND order_status<>99 limit 1", para).FirstOrDefault();
        }

        public bool UpdateOpenId(uint orderId, string oldOpenId, string newOpenId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            para.Add(new StatementParameter { Name = "@NewOpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = newOpenId });
            para.Add(new StatementParameter { Name = "@OldOpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = oldOpenId });
            var sql = "UPDATE orders SET user_openid=@NewOpenId WHERE order_id=@OId AND  user_openid=@OldOpenId LIMIT 1";

            return _baseDao.ExecNonQuery(sql, para) == 1;
        }

        public bool UpdatePrePayInfo(uint orderId, DateTime expiredTime, string prePayId, DateTime lastChangeTime)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            para.Add(new StatementParameter { Name = "@ExpiredTime", Direction = ParameterDirection.Input, DbType = DbType.String, Value = expiredTime.ToString("yyyy-MM-dd HH:mm:ss") });
            para.Add(new StatementParameter { Name = "@PrePayId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = prePayId });
            para.Add(new StatementParameter { Name = "@LastChangeTime", Direction = ParameterDirection.Input, DbType = DbType.String, Value = lastChangeTime.ToString("yyyy-MM-dd HH:mm:ss") });
            var sql = "UPDATE orders SET prepay_id=@PrePayId,expired_time=@ExpiredTime WHERE order_id=@OId AND last_change_time=@LastChangeTime LIMIT 1";

            return _baseDao.ExecNonQuery(sql, para) == 1;
        }

        public bool UpdatePayResult(uint orderId, decimal userPay, string tradeNo)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为30秒
            transactionOption.Timeout = new TimeSpan(0, 0, 30);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                if (!IsStatusChangeAcceptable(OrderStatus.已支付, OrderStatus.未付款))
                {
                    return false;
                }

                var para = new StatementParameterCollection();
                para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
                para.Add(new StatementParameter { Name = "@PayFee", Direction = ParameterDirection.Input, DbType = DbType.Decimal, Value = userPay });
                para.Add(new StatementParameter { Name = "@OldSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.未付款 });
                para.Add(new StatementParameter { Name = "@NewSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.已支付 });

                var sql = "UPDATE orders SET user_pay_fee=@PayFee,order_status=@NewSt WHERE order_id=@OId AND order_status=@OldSt LIMIT 1";

                if (_baseDao.ExecNonQuery(sql, para) == 0)
                {
                    return false;
                }

                var sql2 = "UPDATE order_details SET status=@NewSt WHERE order_id=@OId";

                if (_baseDao.ExecNonQuery(sql2, para) == 0)
                {
                    return false;
                }

                if (DalFactory.Payment.AddPayment(orderId, userPay, tradeNo))
                {
                    ts.Complete();
                    return true;
                }

                return false;
            }
        }

        public IList<OrderDetailEntity> GetSubOrdersByProviderId(int id, int status)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@PId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = id });
            para.Add(new StatementParameter { Name = "@St", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = status });

            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE provider_id=@PId AND status=@St", para);
        }

        public IList<OrderDetailEntity> GetSubOrdersByProviderId(int id, int status, DateTime startTiem, DateTime endTime)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@PId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = id });
            para.Add(new StatementParameter { Name = "@St", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = status });

            var timeFormat = "yyyy-MM-dd HH:mm:ss";

            return _baseDao.SelectList<OrderDetailEntity>((
                "SELECT * FROM order_details WHERE provider_id=@PId AND status=@St " +
                "AND UNIX_TIMESTAMP(create_time)>=UNIX_TIMESTAMP('{0}') AND " +
                "UNIX_TIMESTAMP(create_time)<=UNIX_TIMESTAMP('{1}')")
                .FormatedWith(startTiem.ToString(timeFormat), endTime.ToString(timeFormat)), para);
        }

        public IList<OrderDetailEntity> GetSubOrdersByStatus(int status)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@St", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = status });

            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE status=@St", para);
        }

        public IList<OrderEntity> GetOrdersByStatus(OrderStatus status)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@St", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)status });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_status=@St AND order_status<>99", para);
        }

        public bool CancelOrder(string openId, uint orderId, out int oldStatus)
        {
            oldStatus = -1;
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为30秒
            transactionOption.Timeout = new TimeSpan(0, 0, 30);
            var result = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                var para = new StatementParameterCollection();
                para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
                para.Add(new StatementParameter { Name = "@CancelStatus", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.订单取消 });
                para.Add(new StatementParameter { Name = "@DeleteStatus", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.Deleted });

                var order = _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_id=@OId  AND order_status<>@DeleteStatus limit 1", para).FirstOrDefault();
                if (order == null)
                {
                    throw new Exception("Invild Order Id");
                }

                oldStatus = order.OrderStatus;
                if (!IsStatusChangeAcceptable(OrderStatus.订单取消, (OrderStatus)oldStatus))
                {
                    throw new Exception("Wrong status change process");
                }

                para.Add(new StatementParameter { Name = "@OpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = openId });
                result = _baseDao.ExecNonQuery("UPDATE orders SET	order_status=@CancelStatus WHERE order_id=@OID AND user_openid=@OpenId", para);
                if (result != 1)
                {
                    return false;
                }

                result = _baseDao.ExecNonQuery("UPDATE order_details SET status=@CancelStatus WHERE order_id=@OID", para);
                if (result < 1)
                {
                    return false;
                }

                ts.Complete();

            }

            return true;
        }

        public bool ChangeSubOrderStatus(uint subOrderId, OrderStatus newStatus, OrderStatus oldStatus)
        {
            if (!IsStatusChangeAcceptable(newStatus, oldStatus))
            {
                throw new Exception("Wrong status change process");
            }

            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@newSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)newStatus });
            para.Add(new StatementParameter { Name = "@oldSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)oldStatus });
            para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = subOrderId });
            var sql = "UPDATE order_details SET status=@newSt WHERE id=@OID AND status=@oldSt";

            return _baseDao.ExecNonQuery(sql, para) == 1;
        }

        public bool BulkChangeSubOrdersStatus(List<uint> subOrderIds, OrderStatus newStatus, OrderStatus oldStatus)
        {
            if (!IsStatusChangeAcceptable(newStatus, oldStatus))
            {
                throw new Exception("Wrong status change process");
            }

            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@newSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)newStatus });
            para.Add(new StatementParameter { Name = "@oldSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)oldStatus });
            var sql = "UPDATE order_details SET status=@newSt WHERE id in ({0}) AND status=@oldSt".FormatedWith(string.Join(",", subOrderIds));

            return _baseDao.ExecNonQuery(sql, para) == subOrderIds.Count;
        }

        public int ChangeSubOrderStatusByOrderId(uint orderId, OrderStatus newStatus, OrderStatus oldStatus)
        {
            if (!IsStatusChangeAcceptable(newStatus, oldStatus))
            {
                throw new Exception("Wrong status change process");
            }

            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@newSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)newStatus });
            para.Add(new StatementParameter { Name = "@oldSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)oldStatus });
            para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            var sql = "UPDATE order_details SET status=@newSt WHERE order_id=@OID AND status=@oldSt";

            return _baseDao.ExecNonQuery(sql, para);
        }

        public OrderEntity GetOrderBySubOrderId(uint subOrderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@SubId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = subOrderId });
            para.Add(new StatementParameter { Name = "@DeleteStatus", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.Deleted });

            var query = _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_id = (SELECT  order_id FROM order_details WHERE id=@SubId AND order_status<>@DeleteStatus)", para);

            return query.FirstOrDefault();
        }

        public bool ChangeOrderStatus(uint orderId, OrderStatus newStatus, OrderStatus oldStatus)
        {
            if (!IsStatusChangeAcceptable(newStatus, oldStatus))
            {
                throw new Exception("Wrong status change process");
            }

            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@newSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)newStatus });
            para.Add(new StatementParameter { Name = "@oldSt", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)oldStatus });
            para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            var sql = "UPDATE orders SET order_status=@newSt WHERE order_id=@OID AND order_status=@oldSt";

            return _baseDao.ExecNonQuery(sql, para) == 1;
        }

        public bool DeleteOrder(string openId, uint orderId)
        {
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            // 设置事务超时时间为30秒
            transactionOption.Timeout = new TimeSpan(0, 0, 30);
            var result = 0;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                var order = GetOrderByOrderId(orderId);
                if (order == null)
                {
                    throw new Exception("Invild OrderID");
                }

                if (!IsStatusChangeAcceptable(OrderStatus.Deleted, (OrderStatus)order.OrderStatus))
                {
                    throw new Exception("Wrong status change process");
                }

                var para = new StatementParameterCollection();
                para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
                para.Add(new StatementParameter { Name = "@OpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = openId });
                para.Add(new StatementParameter { Name = "@DeleteStatus", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)OrderStatus.Deleted });
                result = _baseDao.ExecNonQuery("UPDATE orders SET	order_status=@DeleteStatus WHERE order_id=@OID AND user_openid=@OpenId LIMIT 1", para);
                if (result != 1)
                {
                    return false;
                }

                result = _baseDao.ExecNonQuery("UPDATE order_details SET status=@DeleteStatus WHERE order_id=@OID", para);
                if (result < 1)
                {
                    return false;
                }

                ts.Complete();
            }

            return true;
        }

        public OrderDetailEntity GetSubOrderById(uint subOrderId)
        {
            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE id=" + subOrderId).FirstOrDefault();
        }

        private bool IsStatusChangeAcceptable(OrderStatus newStatus, OrderStatus oldStatus)
        {
            var isAcceptable = (newStatus == OrderStatus.订单异常)
                || (oldStatus == OrderStatus.未付款 && newStatus == OrderStatus.已支付)
                || (oldStatus == OrderStatus.已支付 && newStatus == OrderStatus.商家接单)
                || (oldStatus == OrderStatus.未付款 && newStatus == OrderStatus.订单取消)
                || (oldStatus == OrderStatus.已支付 && newStatus == OrderStatus.订单取消)
                || (oldStatus == OrderStatus.商家接单 && newStatus == OrderStatus.商家已配货)
                || (oldStatus == OrderStatus.商家已配货 && newStatus == OrderStatus.快递已取货)
                || (oldStatus == OrderStatus.快递已取货 && newStatus == OrderStatus.已送到指定位置)
                || (oldStatus == OrderStatus.已送到指定位置 && newStatus == OrderStatus.订单结束)
                || (oldStatus == OrderStatus.订单结束 && newStatus == OrderStatus.Deleted)
                || (oldStatus == OrderStatus.订单取消 && newStatus == OrderStatus.Deleted);

            return isAcceptable;
        }
    }

    public enum OrderStatus
    {
        未付款 = 0,
        已支付 = 1,
        商家接单 = 2,
        商家已配货 = 3,
        快递已取货 = 4,
        已送到指定位置 = 5,
        订单结束 = 6,
        订单取消 = 7,
        订单异常 = 8,
        Deleted = 99
    }


}
