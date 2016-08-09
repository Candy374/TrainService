using DAL.Entity;
using System.Collections.Generic;

namespace DAL.DAO
{
    public class TagsDao : CacheBase<TagEntity>
    {
        public TagsDao() : base(new System.TimeSpan(0, 5, 0)) { }

        public IList<TagEntity> GetTags()
        {
            return CachedTable;
        }
    }
}
