
using Arch.Data.Orm;
using System;
using System.Data;

namespace DAL.Entity
{
    [Serializable]
    [Table(Name = "order_details")]
    public class OrderDetailEntity
    {
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public uint ID { get; set; }

        /// <summary>
        /// Order's ID
        /// </summary>
        [Column(Name = "order_id", ColumnType = DbType.UInt32)]
        public uint OrderId { get; set; }

        [Column(Name = "goods_id", ColumnType = DbType.UInt32)]
        public uint GoodsId { get; set; }

        [Column(Name = "purchase_price", ColumnType = DbType.Decimal)]
        public decimal PurchasePrice { get; set; }

        [Column(Name = "sell_price", ColumnType = DbType.Decimal)]
        public decimal SellPrice { get; set; }

        [Column(Name = "provider_id", ColumnType = DbType.Int32)]
        public int ProviderId { get; set; }
    }

}

