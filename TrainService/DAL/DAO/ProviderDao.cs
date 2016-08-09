using DAL.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DAL.DAO
{
    public class ProviderDao : CacheBase<ProviderEntity>
    {
        public ProviderDao() : base(new System.TimeSpan(0, 5, 0)) { }

        public ProviderEntity Search(uint providerId)
        {
            return CachedTable.Where(d => d.ProviderId == providerId).FirstOrDefault();
        }

        public IEnumerable<ProviderEntity> Search(string providerName)
        {
            return CachedTable.Where(d => d.Name == providerName);
        }
    }
}
