using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Booking;
using ViewModel.Customer;
using ViewModel.Result;
using ViewModel.Search;

namespace Services.Interface
{
	public interface IBookingService
	{
		/// <summary>
		/// 取得客戶與預約列表
		/// </summary>
		/// <param name="SearchModel"></param>
		List<CustomerBookingViewModel> GetCustomerBookingList(CarTeamSearchViewModel SearchModel);

		/// <summary>
		/// 發佈班表更新預約列表
		/// </summary>
		/// <param name="PublishScheduleInputModelList"></param>
		/// <returns></returns>
		Task<PublishScheduleVerifyResult> PublishToUpdateBooking(List<PublishScheduleInputViewModel> PublishScheduleInputModelList);

		/// <summary>
		/// 依照訂單Id查訂單狀態
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BookingStatusViewModel> GetBookingStatusList(BookingIdModel model);
	}
}
