
using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "orders")]
    public class OrderEntity
    {
        /// <summary>
        /// Order's ID
        /// </summary>
        [DataMember]
        [Column(Name = "order_id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 OrderId { get; set; }

        [DataMember]
        [Column(Name = "user_id", ColumnType = DbType.Int32)]
        public int UserId { get; set; }

        [DataMember]
        [Column(Name = "user_openid", ColumnType = DbType.String, Length = 50)]
        public string UserOpenid { get; set; }

        [DataMember]
        [Column(Name = "amount", ColumnType = DbType.Decimal)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 0:外面订单，1:商品订单,2：服务订单
        /// </summary>
        [DataMember]
        [Column(Name = "order_type", ColumnType = DbType.Int32)]
        public int OrderType { get; set; }

        /// <summary>
        /// 订单的文字描述
        /// </summary>
        [DataMember]
        [Column(Name = "order_detail", ColumnType = DbType.String)]
        public string OrderDetail { get; set; }

        [DataMember]
        [Column(Name = "order_create_time", ColumnType = DbType.DateTime)]
        public DateTime OrderCreateTime { get; set; }

        /// <summary>
        /// 订单状态：0：未付款，1：已付款，2：商家已接单，3：商家已配货 4:快递员已取货 5:已经送到指定位置 6：订单结束 7：订单取消 8：异常状态
        /// </summary>
        [DataMember]
        [Column(Name = "order_status", ColumnType = DbType.Int32)]
        public int OrderStatus { get; set; }

        [DataMember]
        [Column(Name = "order_msg", ColumnType = DbType.String)]
        public string OrderMsg { get; set; }

        [DataMember]
        [Column(Name = "order_finish_time", ColumnType = DbType.DateTime)]
        public DateTime OrderFinishTime { get; set; }

        [DataMember]
        [Column(Name = "order_date", ColumnType = DbType.Date)]
        public DateTime OrderDate { get; set; }

        [DataMember]
        [Column(Name = "user_pay_fee", ColumnType = DbType.Decimal)]
        public decimal UserPayFee { get; set; }

        /// <summary>
        /// 0：微信支付
        /// </summary>
        [DataMember]
        [Column(Name = "pay_way", ColumnType = DbType.Int32)]
        public int PayWay { get; set; }

        [DataMember]
        [Column(Name = "refund", ColumnType = DbType.Decimal)]
        public decimal Refund { get; set; }

        [DataMember]
        [Column(Name = "refund_goods", ColumnType = DbType.String)]
        public string RefundGoods { get; set; }

        [DataMember]
        [Column(Name = "contacts", ColumnType = DbType.String, Length = 12)]
        public string Contacts { get; set; }

        [DataMember]
        [Column(Name = "contactsNumber", ColumnType = DbType.String, Length = 24)]
        public string ContactNumber { get; set; }

        [DataMember]
        [Column(Name = "train_number", ColumnType = DbType.String, Length = 8)]
        public string TrainNumber { get; set; }

        [DataMember]
        [Column(Name = "carriage_number", ColumnType = DbType.String, Length = 4)]
        public string CarriageNumber { get; set; }

        [DataMember]
        [Column(Name = "is_delay", ColumnType = DbType.Boolean)]
        public bool IsDelay { get; set; }

        public string GetOrderTypeDisplayName()
        {
            switch (this.OrderType)
            {
                case 0:
                    return "外卖";
                case 1:
                    return "商品";
                case 2:
                    return "服务";
                default:
                    return "未知";
            }
        }

        public string GetOrderStatusDisplayName()
        {
            switch (this.OrderStatus)
            {
                case 0:
                    return "待付款";
                case 1:
                    return "已付款";
                case 2:
                    return "商家已接单";
                case 3:
                    return "商家已配货";
                case 4:
                    return "快递已取货";
                case 5:
                    return "已送到指定地点";
                case 6:
                    return "已完成";
                case 7:
                    return "订单取消";
                case 8:
                    return "订单异常";

                default:
                    return "未知";
            }
        }
    }

}

