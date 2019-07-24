namespace ViewModel.Push
{
	/// <summary>
	/// Android 推播訊息格式 ViewModel
	/// </summary>
	public class AndroidPushMessageViewModel: CommonPushMessageViewModel
	{
		/// <summary>
		/// 大標題
		/// </summary>
		public string title { get; set; }

		/// <summary>
		/// 顯示內容
		/// </summary>
		public string body { get; set; }
	}
}
