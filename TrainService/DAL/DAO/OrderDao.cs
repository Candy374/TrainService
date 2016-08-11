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

        public IList<OrderDetailEntity> GetSubOrders(string orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = Convert.ToInt32(orderId) });

            return _baseDao.SelectList<OrderDetailEntity>("SELECT * FROM order_details WHERE order_id=@OId", para);
        }

        public IList<OrderEntity> GetOrderByOpenId(string openId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OpenId", Direction = ParameterDirection.Input, DbType = DbType.String, Value = openId });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE user_openid=@OpenId", para);
        }

        public OrderEntity GetOrderByOrderId(string orderId)
        {
            var para = new StatementParameterCollection();
            para.Add(new StatementParameter { Name = "@OId", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = Convert.ToUInt32(orderId) });

            return _baseDao.SelectList<OrderEntity>("SELECT * FROM orders WHERE order_id=@OId limit 1", para).FirstOrDefault();
        }
    }
}
