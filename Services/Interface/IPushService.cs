using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Push;
using ViewModel.VerifyViewModel;

namespace Services.Interface
{
	public interface IPushService
	{
		/// <summary>
		/// Android 推播訊息
		/// </summary>
		/// <param name="RegistrationIDList">手機裝置中已註冊的KeyNumber清單</param>
		/// <param name="PushMsg">推播訊息</param>
		/// <returns></returns>
		Task<VerityResult> AndroidPushMessage(List<string> RegistrationIDList, AndroidPushMessageViewModel PushMsg);
	}
}
