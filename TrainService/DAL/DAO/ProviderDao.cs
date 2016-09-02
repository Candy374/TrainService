using DAL.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;

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
            return CachedTable.Where(d => d.Name.Contains(providerName));
        }

        public IEnumerable<ProviderEntity> FuzzySearch(string providerName)
        {
            providerName = providerName.ToUpper();
            return CachedTable.Where(d => d.Name.ToUpper().Contains(providerName));
        }

        public ProviderEntity GetProviderByOpenId(string openId)
        {
            return CachedTable.Where(p => p.OpenIdList.Contains(openId)).FirstOrDefault();
        }

        public List<string> GetOpenIdsByProviderId(int providerId)
        {
            var provider = CachedTable.Where(p => Convert.ToInt32(p.ProviderId) == providerId).FirstOrDefault();
            if (provider == null)
            {
                return new List<string>();
            }

            return provider.OpenIdList;
        }
    }
}
