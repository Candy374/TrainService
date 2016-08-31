using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.DAO
{
    public class TimeTableDao
    {
        internal readonly BaseDaoWithLogger _baseDao = new BaseDaoWithLogger("userdb");
        private Dictionary<string, IList<TimeTableEntity>> _cache;
        private DateTime _expiredTime = DateTime.MinValue;

        public TimeTableDao()
        {
            _cache = new Dictionary<string, IList<TimeTableEntity>>();
        }

        public void OverwriteData(string stationCode, IList<TimeTableEntity> list)
        {
            stationCode = stationCode.ToUpper();
            _baseDao.ExecNonQuery("DELETE FROM timetable WHERE station_code='" + stationCode + "'");
            _baseDao.BulkInsert(list);
            if (!_cache.ContainsKey(stationCode))
            {
                _cache.Add(stationCode, new List<TimeTableEntity>());
            }

            _cache[stationCode] = list;
        }

        public TimeTableEntity Query(string stationCode, string trainNumber)
        {
            Reload();
            stationCode = stationCode.ToUpper();
            if (!_cache.ContainsKey(stationCode))
            {
                throw new KeyNotFoundException("Could not found stationCode=" + stationCode);
            }
            trainNumber = trainNumber.ToUpper();

            return _cache[stationCode].Where(t => t.TrainNumber == trainNumber).FirstOrDefault();
        }

        public void Reload()
        {
            if (DateTime.Now > _expiredTime)
            {
                _expiredTime = DateTime.Now.AddMinutes(15);
                var data = _baseDao.SelectList<TimeTableEntity>("SELECT * FROM timetable");
                var dic = new Dictionary<string, IList<TimeTableEntity>>();
                foreach (var item in data)
                {
                    if (!dic.ContainsKey(item.StationCode))
                    {
                        dic.Add(item.StationCode.ToUpper(), new List<TimeTableEntity>());
                    }

                    dic[item.StationCode.ToUpper()].Add(item);
                }

                _cache = dic;

            }
        }
    }
}
