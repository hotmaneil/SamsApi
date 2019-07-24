using System;

namespace ViewModel.Result
{
	public class MessageResultViewModel
	{
		/// <summary>
		/// 訊息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 結果
		/// </summary>
		public bool Result { get; set; }

		/// <summary>
		/// 欄位名稱
		/// </summary>
		public string FieldName { get; set; }

		/// <summary>
		/// Guid編號
		/// </summary>
		public Guid GUID { get; set; }

		/// <summary>
		/// Int編號
		/// </summary>
		public int IntID { get; set; }

		/// <summary>
		/// 0:正常  9:系統程式錯誤
		/// </summary>
		public int RC { get; set; }
	}
}
