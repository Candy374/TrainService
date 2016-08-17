using Arch.Data;
using Arch.Data.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DAL.DAO
{
    public class CacheBase<T> where T : class, new()
    {
        internal readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");
        internal DateTime _expiredTime = DateTime.MinValue;
        internal IList<T> _list;
        private TimeSpan _minRefreshTimeSpan;
        private string _tableName;

        public IList<T> CachedTable
        {
            get
            {
                RefreshData();
                return _list;
            }
        }

        private CacheBase() { }

        public CacheBase(TimeSpan minRefreshTimeSpan)
        {
            this._minRefreshTimeSpan = minRefreshTimeSpan;
            var attrs = CustomAttributeData.GetCustomAttributes(typeof(T));
            var attr = attrs.Where(a => a.AttributeType == typeof(TableAttribute)).FirstOrDefault();
            if (attr == null)
            {
                throw new ArgumentException("T not has TableAttribute");
            }

            if (attr.NamedArguments.Count < 1)
            {
                throw new ArgumentException("TableAttribute should have table name value");
            }

            foreach (var na in attr.NamedArguments)
            {
                if (na.MemberName == "Name")
                {
                    _tableName = na.TypedValue.Value as string;

                }
            }


            if (string.IsNullOrEmpty(_tableName))
            {
                throw new ArgumentException("Table name is empty!");
            }
        }

        private IList<T> RefreshData()
        {
            if (DateTime.Now > _expiredTime || _list == null)
            {
                _list = _baseDao.SelectList<T>("SELECT * FROM " + _tableName);
                _expiredTime = DateTime.Now.Add(_minRefreshTimeSpan);
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
            _expiredTime = DateTime.Now;
            return _baseDao.Update(data);
        }

        public int ExecNonQuery(string sql)
        {
            _expiredTime = DateTime.Now;
            return _baseDao.ExecNonQuery(sql);
        }
    }
}
