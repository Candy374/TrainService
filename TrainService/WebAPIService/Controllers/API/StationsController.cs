using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPIService.Controllers
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api")]
    public class StationsController : ApiController
    {
        [Route("Stations")]
        public IEnumerable<object> Get()
        {
            return _Get();
        }

        public static IEnumerable<object> _Get()
        {
            return DAL.DalFactory.Stations.CachedTable;
        }

        [Route("Stations/{stationCode}/TrainSchedule/{trainNumber}/ArriveTime")]
        public string GetTrainArriveTime(string stationCode, string trainNumber)
        {
            var result = DAL.DalFactory.TimeTable.Query(stationCode, trainNumber);
            if (result != null)
            {
                return result.ArriveTime;
            }

            return "-";
        }

        [Route("Stations/{stationCode}/TrainSchedule/{trainNumber}/TimeCheck")]
        public string CheckTrainArriveTime(string stationCode, string trainNumber)
        {
            var result = DAL.DalFactory.TimeTable.Query(stationCode, trainNumber);
            if (result != null)
            {
                string arriveTime;
                if (result.ArriveTime == "----")
                {
                    arriveTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + result.LeaveTime + ":00";
                }
                else
                {
                    arriveTime = DateTime.Now.ToString("yyyy-MM-dd") + " " + result.ArriveTime + ":00";
                }

                var arrTime = DateTime.Parse(arriveTime);

                if (result.ArriveTime == "----")
                {
                    arrTime = arrTime.AddMinutes(-15);
                }

                if (arrTime - DateTime.Now < new TimeSpan(0, 45, 0))
                {
                    return "Err1:此车次正点到达车站的时间距离现在小于45分钟，无法配送";
                }

                return "OK";
            }

            return "Err2:查询不到该列车信息";
        }

        [Route("Stations/{stationCode}/Upload")]
        public int UploadData(string stationCode, [FromBody]dynamic obj)
        {
            stationCode = stationCode.ToUpper();
            var data = obj.data.data;
            List<DAL.Entity.TimeTableEntity> list = new List<DAL.Entity.TimeTableEntity>();
            foreach (var item in data)
            {
                if (item.station_telecode != stationCode)
                {
                    continue;
                }
                string trainNumber = item.station_train_code;
                trainNumber = trainNumber.ToUpper();
                list.Add(new DAL.Entity.TimeTableEntity
                {
                    ArriveTime = item.arrive_time,
                    LeaveTime = item.start_time,
                    Overstop = item.stopover_time,
                    StationCode = item.station_telecode,
                    TrainNumber = trainNumber,
                    TrainType = trainNumber.Substring(0, 1)
                });
            }

            DAL.DalFactory.TimeTable.OverwriteData(stationCode, list);

            return 0;
        }
    }
}
