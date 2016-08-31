using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "timetable")]
    public class TimeTableEntity
    {

        [DataMember]
        [Column(Name = "station_code", ColumnType = DbType.String, Length = 6)]
        public string StationCode { get; set; }

        [DataMember]
        [Column(Name = "train_type", ColumnType = DbType.String, Length = 1)]
        public string TrainType { get; set; }

        [DataMember]
        [Column(Name = "arrive_time", ColumnType = DbType.String, Length = 6)]
        public string ArriveTime { get; set; }

        [DataMember]
        [Column(Name = "leave_time", ColumnType = DbType.String, Length = 6)]
        public string LeaveTime { get; set; }

        [Column(Name = "train_number", ColumnType = DbType.String, Length = 6)]
        [DataMember]
        public string TrainNumber { get; set; }

        [Column(Name = "overstop", ColumnType = DbType.Int32)]
        public int Overstop { get; set; }
    }
}
