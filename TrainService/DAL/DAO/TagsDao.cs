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
    public class TagsDao : CacheBase<TagEntity>
    {
        public IEnumerable<TagEntity> GetTags()
        {
            return base.Get("SELECT * FROM tags");
        }
    }
}
