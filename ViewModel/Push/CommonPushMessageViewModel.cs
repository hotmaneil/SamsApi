using System;

namespace ViewModel.Push
{
	/// <summary>
	/// 推播訊息格式 共用ViewModel
	/// </summary>
	public class CommonPushMessageViewModel
	{
		/// <summary>
		/// 分類
		/// </summary>
		private int _category = 0;

		/// <summary>
		/// 分類
		/// </summary>
		public int category
		{
			get { return this._category; }
			set
			{
				if (_category != value)
					this._category = value;
			}
		}

		/// <summary>
		/// 訂單編號
		/// </summary>
		public string messageid { get; set; }

		/// <summary>
		/// 發送者id
		/// </summary>
		public string senderid { get; set; }

		/// <summary>
		/// 接收者id
		/// </summary>
		public string readerid { get; set; }

		/// <summary>
		/// 推播發送UTC時間
		/// </summary>
		public DateTime pushtime { get; set; }
	}
}
