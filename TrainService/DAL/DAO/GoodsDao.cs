using DAL.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

namespace DAL.DAO
{
    public class GoodsDao : CacheBase<GoodsEntity>
    {
        public GoodsDao() : base(new System.TimeSpan(0, 5, 0)) { }

        public IEnumerable<GoodsEntity> GetGoodsListByGoodsType(string stationCode, int goodsType)
        {
            var list = base.CachedTable.Where(d => d.StationCode == stationCode && d.GoodsType == goodsType && d.IsAvailable == true && d.IsObsolete == false);

            return list;
        }

        public GoodsEntity GetGoods(uint id)
        {
            return base.CachedTable.Where(d => d.GoodsId == id).FirstOrDefault();
        }
    }
}
