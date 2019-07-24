using Newtonsoft.Json;
using Serilog;
using Services.Interface;
using Services.Service;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ViewModel.Booking;
using ViewModel.Push;
using ViewModel.Result;
using ViewModel.VerifyViewModel;

namespace SamsApi.Controllers
{
	/// <summary>
	/// 班表 控制器
	/// </summary>
	[RoutePrefix("api/CareShfitSchedule")]
	public class CareShfitScheduleController : BaseApiController
	{
		readonly IBookingService _bookingService;
		readonly IPushService _pushService;

		private ILogger _logger = Log.Logger;

		/// <summary>
		/// 班表 控制器
		/// </summary>
		public CareShfitScheduleController()
		{
			_bookingService = new BookingService();
			_pushService = new PushService();
		}

		/// <summary>
		/// 匯入班表
		/// </summary>
		/// <param name="PublishScheduleInputModel"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("ImportDailyDriverSchedule")]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public async Task<IHttpActionResult> ImportDailyDriverSchedule(List<PublishScheduleInputViewModel> PublishScheduleInputModel)
		{
			_logger.Information("ImportDailyDriverSchedule model: " + JsonConvert.SerializeObject(PublishScheduleInputModel));

			PublishScheduleVerifyResult publishScheduleresult = new PublishScheduleVerifyResult();

			try
			{
				VerityResult result = new VerityResult();
				result = JudgeUserIdentifyNameToVerityResult();
				if (!result.IsOk)
					return ReturnResponseMessageResult(result, HttpStatusCode.Unauthorized);

				publishScheduleresult = await _bookingService.PublishToUpdateBooking(PublishScheduleInputModel);
				_logger.Information("ImportDailyDriverSchedule result: " + JsonConvert.SerializeObject(publishScheduleresult));

				foreach (var pushItem in publishScheduleresult.PublishScheduleResultList)
				{
					AndroidPushMessageViewModel pushMsg = new AndroidPushMessageViewModel();
					pushMsg.title = "平台發佈了新班表!";
					pushMsg.body = "請大家確認" + pushItem.BookingDate + "班表😎";
					pushMsg.readerid = pushItem.DriverId.ToString();
					List<string> RegistrationIDList = new List<string>();
					RegistrationIDList.Add(pushItem.KeyNumber);
					await _pushService.AndroidPushMessage(RegistrationIDList, pushMsg);
				}

				return new ResponseMessageResult(
					Request.CreateResponse(
					HttpStatusCode.OK,
					publishScheduleresult
				));
			}
			catch (Exception ex)
			{
				_logger.Information("ImportDailyDriverSchedule Error: " + JsonConvert.SerializeObject(ex));

				publishScheduleresult.VerityResult.IsOk = false;
				publishScheduleresult.VerityResult.Message = ex.Message.ToString();

				return new ResponseMessageResult(
					Request.CreateResponse(
					HttpStatusCode.InternalServerError,
					publishScheduleresult
				));
				throw ex;
			}
		}

	}
}
