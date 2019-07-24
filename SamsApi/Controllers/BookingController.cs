using SamsApi.BLL;
using Services.Interface;
using Services.Service;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using ViewModel.Booking;
using ViewModel.Customer;
using ViewModel.Search;
using ViewModel.VerifyViewModel;

namespace SamsApi.Controllers
{
	/// <summary>
	/// 預約 API控制器
	/// </summary>
	[RoutePrefix("api/Booking")]
	public class BookingController : BaseApiController
	{
		private AuthBLL _authBLL = new AuthBLL();
		readonly IBookingService _bookingService;
		readonly IUsersService _usersService;

		/// <summary>
		/// 預約 API控制器
		/// </summary>
		public BookingController()
		{
			_bookingService = new BookingService();
			_usersService = new UsersService();
		}

		/// <summary>
		/// 取得預約任務
		/// </summary>
		/// <param name="SearchModel"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("GetDailyBookings")]
		[ResponseType(typeof(CustomerBookingViewModel))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		public async Task<IHttpActionResult> GetDailyBookings(CarTeamSearchViewModel SearchModel)
		{
			List<CustomerBookingViewModel> data = new List<CustomerBookingViewModel>();

			VerityResult result = new VerityResult();
			result = JudgeUserIdentifyNameToVerityResult();
			if (!result.IsOk)
				return ReturnResponseMessageResult(result, HttpStatusCode.Unauthorized);

			var user = await _authBLL.FindByName(User.Identity.Name);

			if (user != null)
			{
				int ? queryGroupId= _usersService.GetGroupId(user.Id);

				if (queryGroupId.HasValue)
					SearchModel.GroupId = queryGroupId.Value;

				data = _bookingService.GetCustomerBookingList(SearchModel);
			}
				
			return new ResponseMessageResult(
				Request.CreateResponse(
					HttpStatusCode.OK,
					data
			));
		}

		/// <summary>
		/// 查詢趟次狀態
		/// </summary>
		/// <param name="SearchModel"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("QueryTripStatus")]
		[ResponseType(typeof(CustomerBookingViewModel))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		public async Task<IHttpActionResult> QueryTripStatus(BookingIdModel SearchModel)
		{
			BookingStatusViewModel data = new BookingStatusViewModel();

			VerityResult result = new VerityResult();
			result = JudgeUserIdentifyNameToVerityResult();
			if (!result.IsOk)
				return ReturnResponseMessageResult(result, HttpStatusCode.Unauthorized);

			data = await _bookingService.GetBookingStatusList(SearchModel);

			return new ResponseMessageResult(
				Request.CreateResponse(
					HttpStatusCode.OK,
					data
			));
		}
	}
}
