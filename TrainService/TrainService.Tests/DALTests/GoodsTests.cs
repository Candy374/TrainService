using DAL.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainService.Tests.DALTests
{
    [TestClass]
    public class GoodsTests
    {
        [TestMethod]
        public void AddGoodsTest()
        {
            GoodsEntity entity = new GoodsEntity
            {
                CanChangeFlavor = false,
                GoodsType = 0,
                IsAvailable = true,
                IsObsolete = false,
                Name = "test yu xiang rou si",
                OrderCount = 0,
                PictureUrl = "http://asdasdasdasdasdasdasd",
                ProviderId = 1,
                PurchasePrice = 12.00M,
                Rating = 5,
                SellPrice = 15.00M
            };

            DAL.DalFactory.Goods.Add(entity);
        }
    }
}
