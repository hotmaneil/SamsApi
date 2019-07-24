using ResourceLibrary;
using Services.Interface;
using SQLModel.Models.BarrierFreeTMSModels;
using SQLModel.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utility.Extensions;
using ViewModel.PutTrip;
using ViewModel.VerifyViewModel;

namespace Services.Service
{
	/// <summary>
	/// 放趟 Service
	/// </summary>
	public class PutTripService: IPutTripService
	{
		private IGenericRepository<PutTrip> _putTripRepository = new TMSGenericRepository<PutTrip>();

		readonly IAspNetUsersService _aspNetUsersService;
		readonly IUsersService _usersService;

		protected BarrierFreeTMSEntities _db
		{
			get;
			private set;
		}

		public PutTripService()
		{
			this._db = new BarrierFreeTMSEntities();
			_aspNetUsersService = new AspNetUsersService();
			_usersService = new UsersService();
		}

		/// <summary>
		/// 新增或更新 放趟
		/// </summary>
		/// <param name="model"></param>
		/// <param name="UserId"></param>
		/// <returns></returns>
		public async Task<VerityResult> CreateOrUpdatePutTrip(TripInputViewModel model,string UserId)
		{
			VerityResult result = new VerityResult();

			using (var transaction = _putTripRepository._context.Database.BeginTransaction())
			{
				try
				{
					PutTrip actionItem = new PutTrip();
					int ? groupId = _usersService.GetGroupId(UserId);
					var queryPutTrip = _db.PutTrip.Where(x => x.date.Value == model.date.Date && x.groupId == groupId);
					if (!queryPutTrip.Any())
					{
						DbSetExt.CopyObject<PutTrip>(model, ref actionItem);

						var maxTripId = _db.PutTrip.Max(x => x.tripId);
						string generTripId = GetSerialNumber(maxTripId);

						actionItem.username = UserId;
						actionItem.groupId = _usersService.GetGroupId(UserId);
						actionItem.date = model.date;
						actionItem.createTime = DateTime.Now;
						actionItem.tripId = generTripId;
						actionItem.type = true;
						actionItem.isClose = true;
						actionItem.AuthorizeType = model.AuthotrizeType;

						_putTripRepository.Create(actionItem);

						result.IsOk = true;
						result.Message = string.Format(Resource.CreateSuccess, "PutTrip");
					}
					else
					{
						actionItem = queryPutTrip.First();

						#region 驗證是否是自己的車隊 groupId
						int ? queryDriverGroupID = _usersService.GetGroupId(UserId);
						if (queryDriverGroupID.HasValue)
						{
							if (actionItem.groupId == queryDriverGroupID.Value)
							{
								DbSetExt.CopyObject<PutTrip>(model, ref actionItem);
								actionItem.isClose = false;
								actionItem.AuthorizeType = model.AuthotrizeType;
								actionItem.updateTime = DateTime.Now;

								_putTripRepository.Update(actionItem);

								result.IsOk = true;
								result.Message = string.Format(Resource.UpdateSuccess, "PutTrip");
							}
							else
							{
								DbSetExt.CopyObject<PutTrip>(model, ref actionItem);

								var maxTripId = _db.PutTrip.Max(x => x.tripId);
								string generTripId = GetSerialNumber(maxTripId);

								actionItem.username = UserId;
								actionItem.groupId = _usersService.GetGroupId(UserId);
								actionItem.date = model.date;
								actionItem.createTime = DateTime.Now;
								actionItem.tripId = generTripId;
								actionItem.type = true;
								actionItem.isClose = true;
								actionItem.AuthorizeType = model.AuthotrizeType;

								_putTripRepository.Create(actionItem);

								result.IsOk = true;
								result.Message = string.Format(Resource.CreateSuccess, "PutTrip");
							}
						}
						#endregion
					}
					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					result.Message = ex.Message.ToString();
					result.IsOk = false;
					throw ex;
				}
			}
			return await Task.Run(() => result);
		}

		/// <summary>
		/// 產生流水號：6位日期加8位順序號，如17030200000032。  
		/// </summary>
		/// <param name="SerialNumber"></param>
		/// <returns></returns>
		private string GetSerialNumber(string SerialNumber)
		{
			if (SerialNumber == null)
				return DateTime.Now.ToString("yyMMdd") + "00000001";

			if (SerialNumber != "0")
			{
				string headDate = SerialNumber.Substring(0, 6);
				int lastNumber = int.Parse(SerialNumber.Substring(8));

				//如果數據庫最大值流水號的日期和生成日期在同一天，则順序加1
				if (headDate == DateTime.Now.ToString("yyMMdd"))
				{
					lastNumber++;
					return headDate + lastNumber.ToString("00000000");
				}
			}
			return DateTime.Now.ToString("yyMMdd") + "00000001";
		}
	}
}
