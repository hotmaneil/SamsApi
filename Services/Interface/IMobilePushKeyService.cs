using SQLModel.Models;
using SQLModel.Models.BarrierFreeTMSModels;
using ViewModel.Result;

namespace Services.Interface
{
	public interface IMobilePushKeyService
	{
		/// <summary>
		/// 依照使用者ID取得MobilePushKey
		/// </summary>
		/// <param name="UserID"></param>
		/// <returns></returns>
		MobilePushKey GetByUserID(string UserID);

		/// <summary>
		/// 新增或更新MobilePushKey
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="KeyNumber"></param>
		/// <param name="MobileType"></param>
		/// <returns></returns>
		MessageResultViewModel CreateOrUpdate(string UserID, string KeyNumber, int MobileType);

		/// <summary>
		/// 依照KeyNumber 取得MobilePushKey
		/// </summary>
		/// <param name="KeyNumber"></param>
		/// <param name="UserID"></param>
		/// <returns></returns>
		MobilePushKey GetByParam(string KeyNumber, string UserID);

		/// <summary>
		/// 清理 KeyNumber
		/// </summary>
		/// <param name="KeyNumber"></param>
		void CleanKeyNumber(string KeyNumber);
	}
}
