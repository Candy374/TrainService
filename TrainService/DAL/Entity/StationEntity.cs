using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "stations")]
    public class StationEntity
    {
     
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 StationId { get; set; }
   
        [DataMember]
        [Column(Name = "name", ColumnType = DbType.String, Length = 24)]
        public string Name { get; set; }

        [DataMember]
        [Column(Name = "code", ColumnType = DbType.String, Length = 12)]
        public string StationCode { get; set; }

        [DataMember]
        [Column(Name = "min_price", ColumnType = DbType.Int32)]
        public int MinPrice { get; set; }

        [Column(Name = "pic_url", ColumnType = DbType.String, Length = 128)]
        [DataMember]
        public string PicUrl { get; set; }
    }
}
