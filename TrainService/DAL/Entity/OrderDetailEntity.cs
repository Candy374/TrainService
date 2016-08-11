
using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "order_details")]
    public class OrderDetailEntity
    {
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public uint ID { get; set; }

        /// <summary>
        /// Order's ID
        /// </summary>
        [DataMember]
        [Column(Name = "order_id", ColumnType = DbType.UInt32)]
        public uint OrderId { get; set; }

        [DataMember]
        [Column(Name = "goods_id", ColumnType = DbType.UInt32)]
        public uint GoodsId { get; set; }

        [DataMember]
        [Column(Name = "purchase_price", ColumnType = DbType.Decimal)]
        public decimal PurchasePrice { get; set; }

        [DataMember]
        [Column(Name = "sell_price", ColumnType = DbType.Decimal)]
        public decimal SellPrice { get; set; }

        [DataMember]
        [Column(Name = "provider_id", ColumnType = DbType.Int32)]
        public int ProviderId { get; set; }

        [DataMember]
        [Column(Name = "buy_count",ColumnType =DbType.Int32)]
        public int Count { get; set; }

        [DataMember]
        [Column(Name = "refund_count", ColumnType = DbType.Int32)]
        public int RefundCount { get; set; }

        [DataMember]
        [Column(Name = "display_name", ColumnType = DbType.String, Length = 128)]
        public string DisplayName { get; set; }
    }

}

