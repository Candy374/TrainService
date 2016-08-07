using Arch.Data.Orm;
using System;
using System.Data;

namespace DAL.Entity
{
	[Serializable]
	[Table(Name = "provider")]
	public partial class ProviderEntity
	{
		/// <summary>
		/// provider's ID
		/// </summary>
		[Column(Name = "id", ColumnType = DbType.UInt32), ID, PK]
		public UInt32 ProviderId { get; set; }
		/// <summary>
		/// Provider's name
		/// </summary>
		[Column(Name = "name", ColumnType = DbType.String, Length = 50)]
		public string Name { get; set; }
		/// <summary>
		/// telphone number
		/// </summary>
		[Column(Name = "tel", ColumnType = DbType.String, Length = 128)]
		public string TelphoneNumber { get; set; }
		/// <summary>
		/// the owner of the store
		/// </summary>
		[Column(Name = "owner", ColumnType = DbType.String, Length = 12)]
		public string Owner { get; set; }
		/// <summary>
		/// the url of store's picture
		/// </summary>
		[Column(Name = "pic_url", ColumnType = DbType.String, Length = 128)]
		public string PictureUrl { get; set; }
		/// <summary>
		/// the location description of the store
		/// </summary>
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
		[Column(Name = "is_open", ColumnType = DbType.Boolean)]
		public bool IsOpening { get; set; }
	}
}
