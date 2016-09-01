using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO.Tests
{
    [TestClass()]
    public class OrderDaoTests
    {
        [TestMethod()]
        public void GetOrderByOrderIdTest()
        {
            var d = DalFactory.Orders.GetOrderByOrderId(10);
            var s = DalFactory.Orders.GetSubOrders(10);
        }

        [TestMethod()]
        public void GetSubOrdersSummaryTest()
        {
            var data = DalFactory.Orders.GetSubOrdersSummary(18);
        }

        [TestMethod()]
        public void GetOrdersByStatusTest()
        {
            var data = DalFactory.Orders.GetOrdersByStatus(OrderStatus.已支付);

        }

        [TestMethod()]
        public void UpdateOpenIdTest()
        {
            DalFactory.Orders.UpdateOpenId(17, "test_Open_Id", "qqqqqqqqqqqqqqqqqqqqqq");
        }
    }
}