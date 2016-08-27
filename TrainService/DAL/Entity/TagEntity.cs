using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "tags")]
    public partial class TagEntity
    {
        [DataMember]
        [Column(Name = "tag_id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 ID { get; set; }
        [DataMember]
        [Column(Name = "name", ColumnType = DbType.String, Length = 64)]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "display_name", ColumnType = DbType.String, Length = 50)]
        public string DisplayName { get; set; }
    }
}
