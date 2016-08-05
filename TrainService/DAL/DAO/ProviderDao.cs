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
    public class ProviderDao
    {
        readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");

        public ProviderEntity Search(uint providerId)
        {
            var parameters = new StatementParameterCollection();
            parameters.Add(new StatementParameter { Name = "@ID", Direction = ParameterDirection.Input, DbType = DbType.UInt32, Value = providerId });
            var list = _baseDao.SelectList<ProviderEntity>("select * from provider where id=@ID limit 1", parameters);

            return list.FirstOrDefault();
        }

        public ProviderEntity[] Search(string providerName)
        {
            var parameters = new StatementParameterCollection();
            parameters.Add(new StatementParameter { Name = "@Name", Direction = ParameterDirection.Input, DbType = DbType.String, Value = providerName });
            var list = _baseDao.SelectList<ProviderEntity>("select * from provider where name=@Name limit 1", parameters);

            return list.ToArray();
        }

        public void AddProvider(ProviderEntity data)
        {
            _baseDao.Insert(data);
        }
    }
}
