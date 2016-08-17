using Arch.Data;
using Arch.Data.DbEngine;
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
        internal readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");

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
                "order_details.rating AS rating " +
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

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE user_openid=@OpenId", para);
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
            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders  WHERE user_openid=@OpenId ORDER BY order_create_time LIMIT @StartIndex,@PageSize", para);
        }

        public void SetRated(uint orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = orderId });
            _baseDao.ExecNonQuery("UPDATE orders SET is_rated=TRUE WHERE order_id=@OID");
        }

        public OrderEntity GetOrderByOrderId(string orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = Convert.ToUInt32(orderId) });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_id=@OId limit 1", para).FirstOrDefault();
        }

        public IList<OrderDetailEntity> GetSubOrdersByProviderId(int id)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@PId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = id });

            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE provider_id=@PId AND STATUS =1", para);
        }

        public IList<OrderEntity> GetOrdersByStatus(OrderStatus status)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@St", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = (int)status });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_status=@St", para);
        }

        public bool CancelOrder(uint orderId)
        {
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

                result = _baseDao.ExecNonQuery("UPDATE orders SET	order_status=7 WHERE order_id=@OID AND order_status IN (0,1)", para);
                ts.Complete();
            }

            return result == 1;
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
        订单异常 = 8
    }


}
