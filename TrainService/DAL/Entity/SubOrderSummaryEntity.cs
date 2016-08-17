
using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "order_details")]
    public class SubOrderSummaryEntity
    {
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32)]
        public uint Id { get; set; }

        [DataMember]
        [Column(Name = "url", ColumnType = DbType.String)]
        public string PicUrl { get; set; }

        [DataMember]
        [Column(Name = "sell_price", ColumnType = DbType.Decimal)]
        public decimal SellPrice { get; set; }

        [DataMember]
        [Column(Name = "buy_count", ColumnType = DbType.Int32)]
        public int Count { get; set; }

        [DataMember]
        [Column(Name = "refund_count", ColumnType = DbType.Int32)]
        public int RefundCount { get; set; }

        [DataMember]
        [Column(Name = "display_name", ColumnType = DbType.String, Length = 128)]
        public string DisplayName { get; set; }

        [DataMember]
        [Column(Name = "goods_id", ColumnType = DbType.UInt32)]
        public uint GoodsId { get; set; }

        [DataMember]
        [Column(Name = "rating", ColumnType = DbType.Int32)]
        public int Rate { get; set; }
    }

}

