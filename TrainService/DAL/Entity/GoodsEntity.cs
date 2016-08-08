using Arch.Data.Orm;
using System;
using System.Data;

namespace DAL.Entity
{
    [Serializable]
    [Table(Name = "goods")]
    public partial class GoodsEntity
    {
        /// <summary>
        /// provider's ID
        /// </summary>
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 GoodsId { get; set; }
        /// <summary>
        /// Goods' name
        /// </summary>
        [Column(Name = "name", ColumnType = DbType.String, Length = 128)]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "purchase_price", ColumnType = DbType.Decimal)]
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "sell_price", ColumnType = DbType.Decimal)]
        public decimal SellPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "provider_id", ColumnType = DbType.Int32)]
        public int ProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "can_change_flavor", ColumnType = DbType.Boolean)]
        public bool CanChangeFlavor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "pic_url", ColumnType = DbType.String, Length = 256)]
        public string PictureUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "is_obsolete", ColumnType = DbType.Boolean)]
        public bool IsObsolete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "is_available", ColumnType = DbType.Boolean)]
        public bool IsAvailable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "rating", ColumnType = DbType.Int32)]
        public int Rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "linked_goods_id", ColumnType = DbType.Int32)]
        public int LinkedGoodsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(Name = "order_count", ColumnType = DbType.Int32)]
        public int OrderCount { get; set; }
        /// <summary>
        /// the type of the goods. 0: food, 1:goods, 2:service
        /// </summary>
        [Column(Name = "goods_type", ColumnType = DbType.Int32)]
        public int GoodsType { get; set; }
    }
}
