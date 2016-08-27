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

        public bool Rate(Dictionary<uint, int> goodsRates, Dictionary<uint, int> subOrderRates)
        {
            try
            {
                foreach (var id in goodsRates.Keys)
                {
                  _baseDao.ExecNonQuery("UPDATE goods  SET rating = rating+" + goodsRates[id] + "  WHERE id = " + id);
                }

                foreach (var id in subOrderRates.Keys)
                {
                    _baseDao.ExecNonQuery("UPDATE order_details SET rating=" + subOrderRates[id] + " WHERE id = " + id);

                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
