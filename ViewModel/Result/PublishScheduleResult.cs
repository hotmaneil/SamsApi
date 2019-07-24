using System;

namespace ViewModel.Result
{
	/// <summary>
	/// 發佈班表驗證結果
	/// </summary>
	public class PublishScheduleResult
	{
		/// <summary>
		/// 預約Id
		/// </summary>
		public int BookingId { get; set; }

		/// <summary>
		/// 發佈日期
		/// </summary>
		public DateTime BookingDate { get; set; }

		/// <summary>
		/// 呼號/隊編
		/// </summary>
		public string TaxiCallNo { get; set; }

		/// <summary>
		/// 車牌號碼
		/// </summary>
		public string LicensePlateNumber { get; set; }

		/// <summary>
		/// 司機Id
		/// </summary>
		public int DriverId { get; set; }

		/// <summary>
		/// 司機姓名
		/// </summary>
		public string DriverName { get; set; }

		/// <summary>
		/// 手機推播碼
		/// </summary>
		public string KeyNumber { get; set; }
	}
}
