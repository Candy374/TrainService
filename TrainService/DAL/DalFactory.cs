using DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Foundation;

namespace DAL
{
    public static class DalFactory
    {
        public static readonly ProviderDao Provider = new ProviderDao();
        public static readonly GoodsDao Goods = new GoodsDao();

        public static readonly AccountDao Account = new AccountDao();

    }
}
