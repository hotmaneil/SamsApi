using SQLModel.Models.BarrierFreeTMSModels;
using System.Threading.Tasks;

namespace Services.Interface
{
	public interface IAspNetUsersService
	{
		/// <summary>
		/// 非同步依UserID查使用者
		/// </summary>
		/// <param name="UserID"></param>
		/// <returns></returns>
		Task<AspNetUsers> QueryUsersByIDAsync(string UserID);

		/// <summary>
		/// 依行動電話號碼查使用者
		/// </summary>
		/// <param name="PhoneNumber"></param>
		/// <returns></returns>
		AspNetUsers QueryUsersByPhoneNumber(string PhoneNumber);

		/// <summary>
		/// 依照UserName查詢使用者
		/// </summary>
		/// <param name="UserName"></param>
		/// <returns></returns>
		AspNetUsers QueryUser(string UserName);

		/// <summary>
		/// 由平台帳號取得TaxiCompanyGroupId公司群組編號
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		int? GetGroupId(string UserId);
	}
}
