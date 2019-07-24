using System;

namespace ViewModel.Search
{
	/// <summary>
	/// 車隊查詢 ViewModel
	/// </summary>
	public class CarTeamSearchViewModel
	{
		/// <summary>
		/// 車隊Id
		/// </summary>
		public int GroupId { get; set; }

		/// <summary>
		/// 預約日期
		/// </summary>
		public DateTime ? BookingDate { get; set; }
	}
}
