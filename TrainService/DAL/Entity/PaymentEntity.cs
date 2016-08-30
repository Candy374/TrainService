
using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Text;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "payment")]
    public class PaymentEntity
    {
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public uint Id { get; set; }

        /// <summary>
        /// Order's ID
        /// </summary>
        [DataMember]
        [Column(Name = "order_id", ColumnType = DbType.UInt32)]
        public uint OrderId { get; set; }

        [DataMember]
        [Column(Name = "amount", ColumnType = DbType.Decimal)]
        public decimal Amount { get; set; }

        [DataMember]
        [Column(Name = "trade_number", ColumnType = DbType.String, Length = 32)]
        public string TradeNumber { get; set; }

        [DataMember]
        [Column(Name = "need_refund", ColumnType = DbType.Decimal)]
        public decimal NeedRefund { get; set; }

        [DataMember]
        [Column(Name = "refund", ColumnType = DbType.Decimal)]
        public decimal RefundAmount { get; set; }

        [DataMember]
        [Column(Name = "refund_trade_number", ColumnType = DbType.String, Length = 32)]
        public string RefundTradeNumber { get; set; }

        [DataMember]
        [Column(Name = "pay_time", ColumnType = DbType.DateTime)]
        public DateTime PayTime { get; set; }

        private StringBuilder _logBuilder = new StringBuilder();
        [DataMember]
        [Column(Name = "log", ColumnType = DbType.String)]
        public string Log
        {
            get
            {
                return _logBuilder.ToString();
            }
            set
            {
                _logBuilder.AppendLine(value ?? "");
            }
        }
    }

}

