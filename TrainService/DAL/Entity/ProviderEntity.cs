using Arch.Data.Orm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace DAL.Entity
{
    [Serializable]
    [DataContract]
    [Table(Name = "provider")]
    public partial class ProviderEntity
    {
        /// <summary>
        /// provider's ID
        /// </summary>
        [DataMember]
        [Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
        public UInt32 ProviderId { get; set; }
        /// <summary>
        /// Provider's name
        /// </summary>
        [DataMember]
        [Column(Name = "name", ColumnType = DbType.String, Length = 50)]
        public string Name { get; set; }
        /// <summary>
        /// telphone number
        /// </summary>
        [DataMember]
        [Column(Name = "tel", ColumnType = DbType.String, Length = 128)]
        public string TelphoneNumber { get; set; }
        /// <summary>
        /// the owner of the store
        /// </summary>
        [DataMember]
        [Column(Name = "owner", ColumnType = DbType.String, Length = 12)]
        public string Owner { get; set; }
        /// <summary>
        /// the url of store's picture
        /// </summary>
        [DataMember]
        [Column(Name = "pic_url", ColumnType = DbType.String, Length = 128)]
        public string PictureUrl { get; set; }
        /// <summary>
        /// the location description of the store
        /// </summary>
        [DataMember]
        [Column(Name = "location", ColumnType = DbType.String, Length = 128)]
        public string Location { get; set; }
        /// <summary>
        /// bank account type
        /// </summary>
        [Column(Name = "account_type", ColumnType = DbType.Int32)]
        public int BankType { get; set; }
        /// <summary>
        /// account number
        /// </summary>
        [Column(Name = "account", ColumnType = DbType.String, Length = 50)]
        public string BankAccount { get; set; }
        /// <summary>
        /// is store opening or closed
        /// </summary>
        [DataMember]
        [Column(Name = "is_open", ColumnType = DbType.Boolean)]
        public bool IsOpening { get; set; }


        //[DataMember]
        [Column(Name = "openids", ColumnType = DbType.String)]
        public string OpenIds
        {
            get
            {
                return string.Join("|", OpenIdList);
            }
            set
            {
                OpenIdList = (value ?? "").Split('|').ToList();
            }
        }

       // [DataMember]
        public List<string> OpenIdList { get; private set; }
    }
}
