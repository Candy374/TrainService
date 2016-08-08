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
    public class TagsDao
    {
        readonly BaseDao _baseDao = BaseDaoFactory.CreateBaseDao("userdb");

        public IEnumerable<TagEntity> GetTags()
        {
            return _baseDao.SelectList<TagEntity>("SELECT * FROM tags");
        }

        public int AddTage(TagEntity entity)
        {
            return (int)_baseDao.Insert(entity);
        }
    }
}
