
using Arch.Data.Orm;
using System;
using System.Data;

namespace DAL.Entity
{
    [Serializable]
    [Table(Name = "orders")]
    public class OrderEntity
    {
        /// <summary>
        /// Order's ID
        /// </summary>
        [Column(Name = "order_id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 OrderId { get; set; }

        [Column(Name = "user_id", ColumnType = DbType.Int32)]
        public int UserId { get; set; }

        [Column(Name = "user_openid", ColumnType = DbType.String, Length = 50)]
        public string UserOpenid { get; set; }

        [Column(Name = "amount", ColumnType = DbType.Decimal)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 0:外面订单，1:商品订单,2：服务订单
        /// </summary>
        [Column(Name = "order_type", ColumnType = DbType.Int32)]
        public int OrderType { get; set; }

        /// <summary>
        /// 订单的文字描述
        /// </summary>
        [Column(Name = "order_detail", ColumnType = DbType.String)]
        public string OrderDetail { get; set; }

        [Column(Name = "order_create_time", ColumnType = DbType.DateTime)]
        public DateTime OrderCreateTime { get; set; }

        /// <summary>
        /// 订单状态：0：未付款，1：已付款，2：商家已接单，3：商家已配货 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        /// </summary>
        [Column(Name = "order_status", ColumnType = DbType.Int32)]
        public int OrderStatus { get; set; }

        [Column(Name = "order_msg", ColumnType = DbType.String)]
        public string OrderMsg { get; set; }

        [Column(Name = "order_finish_time", ColumnType = DbType.DateTime)]
        public DateTime OrderFinishTime { get; set; }

        [Column(Name = "order_date", ColumnType = DbType.Date)]
        public DateTime OrderDate { get; set; }

        [Column(Name = "user_pay_fee", ColumnType = DbType.Decimal)]
        public decimal UserPayFee { get; set; }

        /// <summary>
        /// 0：微信支付
        /// </summary>
        [Column(Name = "pay_way", ColumnType = DbType.Int32)]
        public int PayWay { get; set; }

        [Column(Name = "refund", ColumnType = DbType.Decimal)]
        public decimal Refund { get; set; }

        [Column(Name = "refund_goods", ColumnType = DbType.String)]
        public string RefundGoods { get; set; }

        [Column(Name = "contacts", ColumnType = DbType.String, Length = 12)]
        public string Contacts { get; set; }

        [Column(Name = "contactsNumber", ColumnType = DbType.String, Length = 24)]
        public string ContactNumber { get; set; }
    }

}

