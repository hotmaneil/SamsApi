namespace ViewModel.Booking
{
	/// <summary>
	/// 發佈班表輸入Model
	/// </summary>
	public class PublishScheduleInputViewModel: BookingIdModel
	{
		/// <summary>
		/// 呼號/隊編
		/// </summary>
		public string TaxiCallNo { get; set; }

		/// <summary>
		/// 司機姓名
		/// </summary>
		public string DriverName { get; set; }
	}
}
