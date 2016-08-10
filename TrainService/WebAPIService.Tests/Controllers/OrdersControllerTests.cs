using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPIService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIService.Tests;
using System.Web.Script.Serialization;

namespace WebAPIService.Controllers.Tests
{
    [TestClass()]
    public class OrdersControllerTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var controller = new OrdersController();
           

            string json = @"{""OpenId"":""dsfsadfsdfasdfasdfasdf"",""TrainNumber"":""G123"",""CarriageNumber"":""6"",""IsDelay"":false,""OrderType"":0,""PayWay"":0,""Comment"":""jia la buyaosuan ff asdasd sdasd"",""Contact"":""zhang san"",""ContactTel"":""18918080808"",""TotalPrice"":119,""List"":[{""Id"":3,""Count"":2},{""Id"":4,""Count"":1},{""Id"":6,""Count"":3},{""Id"":8,""Count"":1}]}";
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = serializer.Deserialize(json, typeof(object));
            controller.Add(obj);
        }

        [TestMethod]
        public void GetOrderListByOpenId()
        {
            var list = (new OrdersController()).Get("dsfsadfsdfasdfasdfasdf");
        }
    }
}