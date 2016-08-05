using DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DalFactory
    {
        public static readonly ProviderDao Provider = new ProviderDao();
    }
}
