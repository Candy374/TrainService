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
    public class AccountDao : CacheBase<AccountEntity>
    {
        public AccountDao() : base(new TimeSpan(0, 0, 30)) { }

        public AccountEntity GetAccount(string openId)
        {
            return CachedTable.Where(a => a.OpenId == openId).FirstOrDefault();
        }

    }
}
