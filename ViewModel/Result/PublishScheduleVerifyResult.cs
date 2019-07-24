using System.Collections.Generic;
using ViewModel.VerifyViewModel;

namespace ViewModel.Result
{
	/// <summary>
	/// 發佈班表驗證結果集合
	/// </summary>
	public class PublishScheduleVerifyResult
	{
		/// <summary>
		/// 驗證結果
		/// </summary>
		public VerityResult VerityResult { get; set; }

		/// <summary>
		/// 發佈班表驗證結果 List
		/// </summary>
		public List<PublishScheduleResult> PublishScheduleResultList { get; set; }

		public PublishScheduleVerifyResult()
		{
			VerityResult = new VerityResult();
			PublishScheduleResultList = new List<PublishScheduleResult>();
		}
	}
}
