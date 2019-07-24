using System.Net;

namespace ViewModel.VerifyViewModel
{
	/// <summary>
	/// 驗證結果
	/// </summary>
	public class VerityResult
	{
		/// <summary>
		/// 結果
		/// </summary>
		public bool IsOk { get; set; }

		/// <summary>
		/// 訊息
		/// </summary>
		public string Message { get; set; }
	}
}
