namespace Services.Interface
{
	public interface IUsersService
	{
		/// <summary>
		/// 由平台帳號取得TaxiCompanyGroupId公司群組編號
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		int? GetGroupId(string UserId);
	}
}
