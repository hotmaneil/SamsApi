using Services.Interface;
using SQLModel.Models.BarrierFreeTMSModels;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
	public class AspNetUsersService: IAspNetUsersService
	{
		protected BarrierFreeTMSEntities _db
		{
			get;
			private set;
		}

		public AspNetUsersService()
		{
			this._db = new BarrierFreeTMSEntities();
		}

		/// <summary>
		/// 非同步依UserID查使用者
		/// </summary>
		/// <param name="UserID"></param>
		/// <returns></returns>
		public async Task<AspNetUsers> QueryUsersByIDAsync(string UserID)
		{
			var data = _db.AspNetUsers.FirstOrDefault(x => x.Id == UserID);
			return await Task.Run(() => data);
		}

		/// <summary>
		/// 依行動電話號碼查使用者
		/// </summary>
		/// <param name="PhoneNumber"></param>
		/// <returns></returns>
		public AspNetUsers QueryUsersByPhoneNumber(string PhoneNumber)
		{
			using (var _db = new BarrierFreeTMSEntities())
			{
				return _db.AspNetUsers.FirstOrDefault(x => x.PhoneNumber == PhoneNumber);
			}
		}

		/// <summary>
		/// 依照UserName查詢使用者
		/// </summary>
		/// <param name="UserName"></param>
		/// <returns></returns>
		public AspNetUsers QueryUser(string UserName)
		{
			AspNetUsers user = new AspNetUsers();
			var query = _db.AspNetUsers.Where(x => x.UserName == UserName);
			if (query.Any())
				user = query.First();

			return user;
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
