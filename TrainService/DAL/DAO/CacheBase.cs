using Arch.Data;
using Arch.Data.DbEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class CacheBase<T> where T : class, new()
    {
        internal readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");
        internal DateTime _expiredTime = DateTime.MinValue;
        internal IList<T> _list;

        public CacheBase() { }

        internal IList<T> Get(string sql, StatementParameterCollection parameters = null)
        {
            if (DateTime.Now > _expiredTime || _list == null)
            {
                if (parameters != null)
                {
                    _list = _baseDao.SelectList<T>(sql, parameters);
                }
                else
                {
                    _list = _baseDao.SelectList<T>(sql);
                }
                _expiredTime = DateTime.Now.AddSeconds(30);
            }

            return _list;
        }

        public object Add(T data)
        {
            _expiredTime = DateTime.Now;
            return _baseDao.Insert(data);
        }

        public int Update(T data)
        {
            return _baseDao.Update(data);
        }
    }
}
