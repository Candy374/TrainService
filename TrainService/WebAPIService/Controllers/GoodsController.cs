using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DAL;
using WebAPIService.Entity;

namespace WebAPIService.Controllers
{

    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api/Goods")]
    public class GoodsController : ApiController
    {
        [Route("{stationCode}/{type}")]
        public IEnumerable<object> Get(string stationCode, int type)
        {
            var list = DalFactory.Goods.GetGoodsListByGoodsType(stationCode, type);

            var ret = new List<UIGoodsEntity>();
            foreach (var item in list)
            {
                ret.Add(new UIGoodsEntity
                {
                    CanChangeFlavor = item.CanChangeFlavor,
                    GoodsId = item.GoodsId,
                    Name = item.Name,
                    OrderCount = item.OrderCount,
                    PictureUrl = item.PictureUrl,
                    ProviderId = item.ProviderId,
                    Rating = item.Rating ?? -1,
                    SellPrice = item.SellPrice,
                    Tags = item.Tags == null ? new int[0] : GetTages(item.Tags)
                });
            }

            return ret;
        }

        private int[] GetTages(string tags)
        {
            var parts = tags.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> nums = new List<int>();
            foreach (var item in parts)
            {
                int x;
                if (int.TryParse(item, out x))
                {
                    nums.Add(x);
                }
            }

            return nums.ToArray();
        }
    }
}

