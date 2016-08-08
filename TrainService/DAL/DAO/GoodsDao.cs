using Arch.Data;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.CFX;
using Arch.CFramework;
using Arch.Data.DbEngine;
using System.Data;

namespace DAL.DAO
{
    public class GoodsDao
    {
        readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");

        public IList<GoodsEntity> GetGoodsListByGoodsType(string stationCode, int goodsType)
        {
            var sql = "SELECT * FROM goods WHERE station_code=@StCode AND goods_type=@GoodsType AND is_obsolete=0 AND is_available=1";
            var parameters = new StatementParameterCollection();
            parameters.Add(new StatementParameter { Name = "@GoodsType", Direction = ParameterDirection.Input, DbType = DbType.Int32, Value = goodsType });
            parameters.Add(new StatementParameter { Name = "@StCode", Direction = ParameterDirection.Input, DbType = DbType.String, Value = stationCode.ToUpper() });

            var list = _baseDao.SelectList<GoodsEntity>(sql, parameters);

            return list;
        }

        public bool Add(GoodsEntity data)
        {
            var d = _baseDao.Insert(data);

            return true;
        }

        public bool Modify(GoodsEntity data)
        {
            var d = _baseDao.Update(data);

            return d == 1;
        }
    }
}
