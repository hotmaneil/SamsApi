using Services.Interface;
using SQLModel.Models;
using SQLModel.Models.BarrierFreeTMSModels;
using System;
using System.Linq;
using ViewModel.Result;

namespace Services.Service
{
	public class MobilePushKeyService: IMobilePushKeyService
	{
		protected BarrierFreeTMSEntities _db
		{
			get;
			private set;
		}

		public MobilePushKeyService()
		{
			this._db = new BarrierFreeTMSEntities();
		}

		/// <summary>
		/// 依照使用者ID取得MobilePushKey
		/// </summary>
		/// <param name="UserID"></param>
		/// <returns></returns>
		public MobilePushKey GetByUserID(string UserID)
		{
			var query = _db.MobilePushKey.FirstOrDefault(x => x.UserId == UserID);
			return query;
		}

		/// <summary>
		/// 新增或更新MobilePushKey
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="KeyNumber"></param>
		/// <param name="MobileType"></param>
		/// <returns></returns>
		public MessageResultViewModel CreateOrUpdate(string UserID, string KeyNumber, int MobileType)
		{
			MessageResultViewModel resultViewModel = new MessageResultViewModel();
			using (var _db = new BarrierFreeTMSEntities())
			{
				try
				{
					var query = _db.MobilePushKey.Where(x => x.UserId == UserID);
					if (!query.Any())
					{
						var actionItem = new MobilePushKey
						{
							KeyNumber = KeyNumber,
							UserId = UserID,
							MobileType = MobileType,
							InsTime = DateTime.Now
						};
						_db.MobilePushKey.Add(actionItem);

						resultViewModel.Result = true;
						resultViewModel.Message = "Create MobilePushKey:" + KeyNumber + "success!";
					}
					else
					{
						MobilePushKey actionItem = new MobilePushKey();
						actionItem.KeyNumber = KeyNumber;
						actionItem.MobileType = MobileType;
						actionItem.updTime = DateTime.Now;

						resultViewModel.Result = true;
						resultViewModel.Message = "Update MobilePushKey:" + KeyNumber + "success!";
					}
					_db.SaveChanges();
				}
				catch (Exception ex)
				{
					resultViewModel.Message = ex.ToString();
					resultViewModel.Result = false;
				}
			}
			return resultViewModel;
		}

		/// <summary>
		/// 依照KeyNumber 取得MobilePushKey
		/// </summary>
		/// <param name="KeyNumber"></param>
		/// <param name="UserID"></param>
		/// <returns></returns>
		public MobilePushKey GetByParam(string KeyNumber, string UserID)
		{
			using (var _db = new BarrierFreeTMSEntities())
			{
				return _db.MobilePushKey.FirstOrDefault(x => x.KeyNumber == KeyNumber && x.UserId == UserID);
			}
		}

		/// <summary>
		/// 清理 KeyNumber
		/// </summary>
		/// <param name="KeyNumber"></param>
		public void CleanKeyNumber(string KeyNumber)
		{
			using (var _db = new BarrierFreeTMSEntities())
			{
				var query = _db.MobilePushKey.Where(x => x.KeyNumber == KeyNumber);
				if (query.Any())
				{
					var existKeyNumberList = query.ToList();
					foreach (var existKeyNumber in existKeyNumberList)
					{
						existKeyNumber.KeyNumber = " ";
						existKeyNumber.MobileType = null;
					}
					_db.SaveChanges();
				}
			}
		}
	}
}
