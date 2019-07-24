using Services.Interface;
using SQLModel.Models.BarrierFreeTMSModels;
using System.Linq;

namespace Services.Service
{
	public class UsersService: IUsersService
	{
		protected BarrierFreeTMSEntities _db
		{
			get;
			private set;
		}

		public UsersService()
		{
			this._db = new BarrierFreeTMSEntities();
		}

		/// <summary>
		/// 由平台帳號取得TaxiCompanyGroupId公司群組編號
		/// </summary>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public int? GetGroupId(string UserId)
		{
			int? groupId = null;
			var query = _db.AspNetUsers.Where(x => x.Id == UserId);
			if (query.Any())
				groupId = query.First().GroupId;

			return groupId;
		}
	}
}
