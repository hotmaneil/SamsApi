using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Customer
{
	/// <summary>
	/// 客戶預約資料  ViewModel
	/// </summary>
	public class CustomerBookingViewModel
	{
		/// <summary>
		/// 訂車編號
		/// </summary>
		public int BookingId { get; set; }

		/// <summary>
		/// 預約日期
		/// </summary>
		public DateTime BookingDate { get; set; }

		/// <summary>
		/// 乘客Id
		/// </summary>
		public string PassengerId { get; set; }

		/// <summary>
		/// 乘客姓名
		/// </summary>
		[Display(Name = "乘客姓名")]
		public string RealName { get; set; }

		/// <summary>
		/// 電話
		/// </summary>
		public string PhoneNumber { get; set; }

		/// <summary>
		/// 身障等級
		/// </summary>
		public int BarriersLevel { get; set; }

		/// <summary>
		/// 是否使用輪椅
		/// </summary>
		public bool Wheelchair { get; set; }

		/// <summary>
		/// 上車地點
		/// </summary>
		[Display(Name = "上車地點")]
		public string Start_Address { get; set; }

		/// <summary>
		/// 下車地點
		/// </summary>
		[Display(Name = "下車地點")]
		public string Target_Address { get; set; }

		/// <summary>
		/// 是否取消
		/// </summary>
		public bool IsCancel { get; set; }

		/// <summary>
		/// 呼號/隊編
		/// </summary>
		[Display(Name = "呼號/隊編")]
		public string TaxiCallNo { get; set; }

		/// <summary>
		/// 司機 Id
		/// </summary>
		public int ? DriverId { get; set; }

		/// <summary>
		/// 司機
		/// </summary>
		[Display(Name = "司機")]
		public string DriverName { get; set; }
	}
}
