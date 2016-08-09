using Arch.Data.Orm;
using System;
using System.Data;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "provider")]
    public partial class AccountEntity
    {
        /// <summary>
        /// provider's ID
        /// </summary>
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 ID { get; set; }
        /// <summary>
        /// 用户是否订阅该公众号
        /// </summary>
        [DataMember]
        [Column(Name = "subscribe", ColumnType = DbType.Boolean)]
        public string IsSubscribed { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [DataMember]
        [Column(Name = "openid", ColumnType = DbType.String, Length = 50)]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        [DataMember]
        [Column(Name = "nickname", ColumnType = DbType.String, Length = 50)]
        public string NickName { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知	
        /// </summary>
        [DataMember]
        [Column(Name = "sex", ColumnType = DbType.Int32)]
        public int Sex { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        [DataMember]
        [Column(Name = "city", ColumnType = DbType.String, Length = 24)]
        public string City { get; set; }
        /// <summary>
        /// 用户所在省份 
        /// </summary>
        [DataMember]
        [Column(Name = "province", ColumnType = DbType.String, Length = 24)]
        public string Province { get; set; }
        /// <summary>
        /// 用户所在国家	
        /// </summary>
        [DataMember]
        [Column(Name = "country", ColumnType = DbType.String, Length = 24)]
        public string Country { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        [DataMember]
        [Column(Name = "language", ColumnType = DbType.String, Length = 10)]
        public string LanguageCode { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小
        /// （有0、46、64、96、132数值可选，
        /// 0代表640*640正方形头像），用户没有头像时该项为空。
        /// 若用户更换头像，原有头像URL将失效。
        /// </summary>
        [DataMember]
        [Column(Name = "headimgurl", ColumnType = DbType.String, Length = 256)]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 用户关注时间。如果用户曾多次关注，则取最后关注时间	
        /// </summary>
        [DataMember]
        [Column(Name = "subscribe_time", ColumnType = DbType.DateTime)]
        public DateTime SubscribeTime { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        [DataMember]
        [Column(Name = "unionid", ColumnType = DbType.String, Length = 50)]
        public string UnionId { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        [DataMember]
        [Column(Name = "remark", ColumnType = DbType.String)]
        public string Remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        [DataMember]
        [Column(Name = "groupid", ColumnType = DbType.Int32)]
        public int GroupID { get; set; }
        /// <summary>
        /// 最后一次使用的联系人姓名
        /// </summary>
        [DataMember]
        [Column(Name = "last_contact_name", ColumnType = DbType.String, Length = 12)]
        public string LastContactName { get; set; }
        /// <summary>
        /// 最后一次使用的联系人电话
        /// </summary>
        [DataMember]
        [Column(Name = "last_contact_tel", ColumnType = DbType.String, Length = 24)]
        public string LastContactTel { get; set; }
    }
}
