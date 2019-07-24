using System;

namespace ViewModel.Booking
{
	/// <summary>
	/// 訂單 狀態 ViewModel
	/// </summary>
	public class BookingStatusViewModel: BookingIdModel
	{
		/// <summary>
		/// 處理狀態
		/// </summary>
		public byte ? ProcessStatus { get; set; }

		/// <summary>
		/// 流程狀態
		/// </summary>
		public int ? FlowStatus { get; set; }

		public DateTime ? GetonTime { get; set; }

		public DateTime ? GetoffTime { get; set; }

		/// <summary>
		/// 抵達時間
		/// </summary>
		public DateTime ? ArrivalTime { get; set; }

		public BookingStatusViewModel()
		{
			ProcessStatus = null;
			FlowStatus = null;
		}
	}
}
