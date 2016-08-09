using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "goods")]
    public partial class GoodsEntity
    {
        /// <summary>
        /// provider's ID
        /// </summary>
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 GoodsId { get; set; }
        /// <summary>
        /// Goods' name
        /// </summary>
        [DataMember]
        [Column(Name = "name", ColumnType = DbType.String, Length = 128)]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "purchase_price", ColumnType = DbType.Decimal)]
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "sell_price", ColumnType = DbType.Decimal)]
        public decimal SellPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "provider_id", ColumnType = DbType.Int32)]
        public int ProviderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "can_change_flavor", ColumnType = DbType.Boolean)]
        public bool CanChangeFlavor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "pic_url", ColumnType = DbType.String, Length = 256)]
        public string PictureUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "is_obsolete", ColumnType = DbType.Boolean)]
        public bool IsObsolete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "is_available", ColumnType = DbType.Boolean)]
        public bool IsAvailable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "rating", ColumnType = DbType.Int32)]
        public int? Rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "linked_goods_id", ColumnType = DbType.Int32)]
        public int? LinkedGoodsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [Column(Name = "order_count", ColumnType = DbType.Int32)]
        public int OrderCount { get; set; }
        /// <summary>
        /// the type of the goods. 0: food, 1:goods, 2:service
        /// </summary>
        [DataMember]
        [Column(Name = "goods_type", ColumnType = DbType.Int32)]
        public int GoodsType { get; set; }

        [DataMember]
        [Column(Name = "tags", ColumnType = DbType.String)]
        public string Tags { get; set; }

        [DataMember]
        [Column(Name = "station_code", ColumnType = DbType.String, Length = 9)]
        public string StationCode { get; set; }
    }
}
