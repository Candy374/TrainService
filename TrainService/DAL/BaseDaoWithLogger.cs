using Arch.Data;
using Arch.Data.DbEngine;
using LoggerContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class BaseDaoWithLogger
    {
        BaseDao _baseDao;
        public BaseDaoWithLogger(string logicDbName)
        {
            _baseDao = BaseDaoFactory.CreateBaseDao(logicDbName);
        }

        //
        // Summary:
        //     执行非查询指令
        //
        // Parameters:
        //   sql:
        //     sql语句
        //
        // Returns:
        //     影响行数
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public int ExecNonQuery(string sql)
        {
            Logger.Info(sql);
            var result = _baseDao.ExecNonQuery(sql);
            Logger.Info("Result:" + result);

            return result;
        }

        //
        // Summary:
        //     执行非查询指令
        //
        // Parameters:
        //   sql:
        //     sql语句
        //
        //   parameters:
        //     查询参数
        //
        // Returns:
        //     影响行数
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public int ExecNonQuery(string sql, StatementParameterCollection parameters)
        {
            Logger.Info(sql + "\r\n" + parameters.ToString2());
            var result = _baseDao.ExecNonQuery(sql, parameters);
            Logger.Info("Result:" + result);

            return result;
        }

        //
        // Summary:
        //     执行查询语句
        //
        // Parameters:
        //   sql:
        //     sql语句
        //
        // Type parameters:
        //   T:
        //     对象类型
        //
        // Returns:
        //     结果集合
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public IList<T> SelectList<T>(string sql) where T : class, new()
        {
            Logger.Info(sql);
            var result = _baseDao.SelectList<T>(sql);
            Logger.Info("Result:" + result);

            return result;
        }

        //
        // Summary:
        //     执行查询语句
        //
        // Parameters:
        //   sql:
        //     sql语句
        //
        //   parameters:
        //     查询参数
        //
        // Type parameters:
        //   T:
        //     对象类型
        //
        // Returns:
        //     结果集合
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public IList<T> SelectList<T>(string sql, StatementParameterCollection parameters) where T : class, new()
        {
            Logger.Info(sql + "\r\n" + parameters.ToString2());
            var result = _baseDao.SelectList<T>(sql, parameters);
            Logger.Info("Result:" + result);

            return result;
        }

        //
        // Summary:
        //     插入对象
        //     示例：
        //     readonly BaseDao baseDao = BaseDaoFactory.CreateBaseDao("your databaseSetName");
        //     City c = new City { Name = "test", Population = "test", Country1Code = "test",
        //     District = "test" }; baseDao.Insert(city);
        //     备注:
        //     1.实体类打上了[ID]这个标签，插入成功将返回新增的主键值
        //     2.实体类打上了[PK]这个标签，(插入成功将返回新增的主键值，如果PK不为ID，则返回影响条数)
        //
        // Parameters:
        //   obj:
        //     实体对象
        //
        // Type parameters:
        //   T:
        //     实体类型
        //
        // Returns:
        //     返回第一列值（譬如自增长ID）
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public object Insert<T>(T obj) where T : class, new()
        {
            return _baseDao.Insert<T>(obj);
        }

        //
        // Summary:
        //     批量插入对象
        //     示例：
        //     readonly BaseDao baseDao = BaseDaoFactory.CreateBaseDao("your databaseSetName");
        //     List( < UserPrizeDrawedResultsEntity > list = new List < UserPrizeDrawedResultsEntity
        //     >(); UserPrizeDrawedResultsEntity item = null; for (int i = 0; i < 100; i++)
        //     { item = new UserPrizeDrawedResultsEntity(); item.PrizeDrawTime ="test"; item.PrizeInfoID
        //     = "test"; item.PrizeActivityId = "test"; item.Uid = "test"; list.Add(item); }
        //     baseDao.BulkInsert(list);
        //     备注:
        //     1.batchSize最大10000，建议100，分批插入。
        //     2.该批量插入推荐在mysql中使用，如果MsSQL推荐用表变量方式处理
        //
        // Parameters:
        //   list:
        //     对象集合
        //
        // Type parameters:
        //   T:
        //     对象类型
        //
        // Returns:
        //     是否成功
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问异常
        public bool BulkInsert<T>(IList<T> list) where T : class, new()
        {
            return _baseDao.BulkInsert<T>(list);
        }

        //
        // Summary:
        //     更新记录
        //
        // Parameters:
        //   obj:
        //     实体对象
        //
        // Type parameters:
        //   T:
        //     对象类型
        //
        // Returns:
        //     影响行数
        //
        // Exceptions:
        //   T:Arch.Data.DalException:
        //     数据访问框架异常
        public int Update<T>(T obj) where T : class, new()
        {
            return _baseDao.Update<T>(obj);
        }
    }
}
