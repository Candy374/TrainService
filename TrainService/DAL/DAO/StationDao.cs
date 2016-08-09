using DAL.Entity;
using System.Collections.Generic;

namespace DAL.DAO
{
    public class StationDao : CacheBase<StationEntity>
    {
        public StationDao() : base(new System.TimeSpan(0, 10, 0)) { }
        public IList<StationEntity> Get()
        {
            return CachedTable;
        }
    }
}
