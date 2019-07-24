using ResourceLibrary;
using Services.Interface;
using SQLModel.Models.BarrierFreePassengerModels;
using SQLModel.Models.BarrierFreeTMSModels;
using SQLModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Booking;
using ViewModel.Customer;
using ViewModel.Enum;
using ViewModel.Result;
using ViewModel.Search;

namespace Services.Service
{
	/// <summary>
	/// 預約 Service
	/// </summary>
	public class BookingService : IBookingService
	{
		private IGenericRepository<Booking> _bookingRepository = new PassengerGenericRepository<Booking>();

		protected BarrierFreePassengerEntities _passengerDb
		{
			get;
			private set;
		}

		protected BarrierFreeTMSEntities _TMSDb
		{
			get;
			private set;
		}

		public BookingService()
		{
			this._passengerDb = new BarrierFreePassengerEntities();
			this._TMSDb = new BarrierFreeTMSEntities();
		}

		/// <summary>
		/// 取得客戶與預約列表
		/// </summary>
		/// <param name="SearchModel"></param>
		/// <returns></returns>
		public List<CustomerBookingViewModel> GetCustomerBookingList(CarTeamSearchViewModel SearchModel)
		{
			List<CustomerBookingViewModel> data = new List<CustomerBookingViewModel>();

			var query = _passengerDb.Booking.Where(x => x.TaxiCompanyGroupId == SearchModel.GroupId).AsQueryable();

			if (SearchModel.BookingDate.HasValue)
			{
				DateTime startTime = new DateTime(SearchModel.BookingDate.Value.Date.Year, SearchModel.BookingDate.Value.Date.Month, SearchModel.BookingDate.Value.Date.Day, 0, 0, 0);
				DateTime endTime = new DateTime(SearchModel.BookingDate.Value.Date.Year, SearchModel.BookingDate.Value.Date.Month, SearchModel.BookingDate.Value.Date.Day, 23, 59, 59);
				query = query.Where(x => x.BookingDate >= startTime && x.BookingDate <= endTime);
			}

			var tempResult =
			from booking in query
			join passenger in _passengerDb.AspNetUsers on booking.PassengerId equals passenger.Id
			join extendBarrier in _passengerDb.UserExtendBarriers on passenger.Id equals extendBarrier.UserId
			select new CustomerBookingViewModel
			{
				BookingId = booking.BookingId,
				BookingDate = booking.BookingDate,
				PassengerId = booking.PassengerId,
				RealName = passenger.RealName,
				PhoneNumber = passenger.PhoneNumber,
				BarriersLevel = extendBarrier.BarriersLevel,
				Wheelchair = extendBarrier.Wheelchair,
				Start_Address = booking.Start_Address,
				Target_Address = booking.Target_Address,
				IsCancel = booking.IsCancel,
				DriverId = booking.DriverId
			};

			if (tempResult.Any())
			{
				var finalResult = tempResult.ToList();
				foreach (var item in finalResult)
				{
					var queryDriver = _TMSDb.Driver.Where(x => x.DriverId == item.DriverId);
					if (queryDriver.Any())
						item.DriverName = queryDriver.First().DriverName;

					var queryTaxi =
						from main in _TMSDb.TaxiDriver.Where(x => x.DriverId == item.DriverId)
						join taxi in _TMSDb.Taxi on main.TaxiId equals taxi.TaxiId
						select taxi;

					if (queryTaxi.Any())
						item.TaxiCallNo = queryTaxi.First().TaxiCallNo;

				}
				data = finalResult;
			}
			return data;
		}

		/// <summary>
		/// 發佈班表更新預約列表
		/// </summary>
		/// <param name="PublishScheduleInputModelList"></param>
		/// <returns></returns>
		public async Task<PublishScheduleVerifyResult> PublishToUpdateBooking(List<PublishScheduleInputViewModel> PublishScheduleInputModelList)
		{
			PublishScheduleVerifyResult result = new PublishScheduleVerifyResult();

			using (var transaction=_bookingRepository._context.Database.BeginTransaction())
			{
				try
				{
					List<PublishScheduleResult> publishScheduleResultList = new List<PublishScheduleResult>();
					List<Booking> bookingItems = new List<Booking>();
					foreach (var item in PublishScheduleInputModelList)
					{
						Booking actionItem = new Booking();
						var query = _bookingRepository.FindBy(x => x.BookingId == item.BookingId);
						if (query.Any())
						{
							actionItem = query.First();

							Taxi taxi = new Taxi();
							var queryTaxi = _TMSDb.Taxi.Where(x => x.TaxiCallNo == item.TaxiCallNo);
							if (queryTaxi.Any())
							{
								taxi = queryTaxi.First();
								actionItem.TaxiId = taxi.TaxiId;
							}

							Driver driver = new Driver();
							var queryDriver = _TMSDb.Driver.Where(x => x.DriverName == item.DriverName);
							if (queryDriver.Any())
							{
								driver = queryDriver.First();
								actionItem.DriverId = driver.DriverId;
							}
								
							actionItem.ProcessStatus = (Byte)BookingStatus.Accept;

							bookingItems.Add(actionItem);

							PublishScheduleResult scheduleResult = new PublishScheduleResult();
							scheduleResult.BookingId = item.BookingId;
							scheduleResult.BookingDate = actionItem.BookingDate;
							scheduleResult.TaxiCallNo = item.TaxiCallNo;
							scheduleResult.LicensePlateNumber = taxi.LicensePlateNumber;
							scheduleResult.DriverId = driver.DriverId;
							scheduleResult.DriverName = driver.DriverName;

							var queryMobilePushKey =
								from main in _TMSDb.AspNetUsers.Where(x => x.NickName == driver.DriverName)
								join mobilePushKey in _TMSDb.MobilePushKey on main.Id equals mobilePushKey.UserId
								select mobilePushKey;

							if (queryMobilePushKey.Any())
								scheduleResult.KeyNumber = queryMobilePushKey.First().KeyNumber;

							publishScheduleResultList.Add(scheduleResult);
						}
					}
					_bookingRepository.UpdateMultiple(bookingItems);
					transaction.Commit();

					result.VerityResult.IsOk = true;
					result.VerityResult.Message = string.Format(Resource.UpdateSuccess, "PublishToUpdateBooking");
					result.PublishScheduleResultList = publishScheduleResultList;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					result.VerityResult.Message = ex.Message.ToString();
					result.VerityResult.IsOk = false;
					throw ex;
				}
			}
			return await Task.Run(() => result);
		}

		/// <summary>
		/// 依照訂單Id查訂單狀態
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<BookingStatusViewModel> GetBookingStatusList(BookingIdModel model)
		{
			BookingStatusViewModel data = new BookingStatusViewModel();
			var query =
				from main in _passengerDb.Booking
				where main.BookingId == model.BookingId
				join status in _passengerDb.DispatchStatus on main.OrderNo equals status.OrderId into statuses
				from statusV in statuses.DefaultIfEmpty()
				select new BookingStatusViewModel
				{
					BookingId = main.BookingId,
					ProcessStatus = main.ProcessStatus,
					FlowStatus = statusV.FlowStatus,
					GetonTime = statusV.GetonTime,
					GetoffTime = statusV.GetoffTime,
					ArrivalTime = statusV.ArrivalTime
				};

			if (query.Any())
				data = query.First();

			return await Task.Run(() => data);
		}
	}
}
