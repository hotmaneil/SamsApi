using Newtonsoft.Json;
using Serilog;
using Services.Interface;
using Services.Service;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using ViewModel.PutTrip;
using ViewModel.VerifyViewModel;

namespace SamsApi.Controllers
{
	/// <summary>
	/// 照護放趟 控制器
	/// </summary>
	[RoutePrefix("api/CarePutTrip")]
	public class CarePutTripController : BaseApiController
	{
		readonly IPutTripService _putTripService;
		readonly IAspNetUsersService _aspNetUsersService;

		private ILogger _logger = Log.Logger;

		/// <summary>
		/// 照護放趟 控制器
		/// </summary>
		public CarePutTripController()
		{
			_putTripService = new PutTripService();
			_aspNetUsersService = new AspNetUsersService();
		}

		/// <summary>
		/// 放趟
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("SetDailyServiceTrips")]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.BadRequest)]
		public async Task<IHttpActionResult> SetDailyServiceTrips(TripInputViewModel model)
		{
			_logger.Information("SetDailyServiceTrips model: " + JsonConvert.SerializeObject(model));

			VerityResult result = new VerityResult();

			try
			{
				result = JudgeUserIdentifyNameToVerityResult();
				if (!result.IsOk)
					return ReturnResponseMessageResult(result, HttpStatusCode.Unauthorized);

				string userId = _aspNetUsersService.QueryUser(User.Identity.Name).Id;
				result = await _putTripService.CreateOrUpdatePutTrip(model, userId);

				_logger.Information("SetDailyServiceTrips result: " + JsonConvert.SerializeObject(result));
				return new ResponseMessageResult(
					Request.CreateResponse(
					HttpStatusCode.OK,
					result
				));
			}
			catch (Exception ex)
			{
				_logger.Information("SetDailyServiceTrips Error: " + JsonConvert.SerializeObject(ex));

				result.IsOk = false;
				result.Message = ex.Message.ToString();
				return ReturnResponseMessageResult(result, HttpStatusCode.InternalServerError);
				throw ex;
			}
		}
	}
}