using ResourceLibrary;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using ViewModel.VerifyViewModel;

namespace SamsApi.Controllers
{
	/// <summary>
	/// 基底 ApiController
	/// </summary>
	public class BaseApiController : ApiController
    {
		private ILogger _logger = Log.Logger;

		/// <summary>
		/// 判斷Token之User Identify Name 是否有效 供 VerityResult 用
		/// </summary>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		public VerityResult JudgeUserIdentifyNameToVerityResult()
		{
			VerityResult verityResult = new VerityResult();
			if (User.Identity.Name == null)
			{
				verityResult.IsOk = false;
				verityResult.Message = Resource.TokenInactive;
			}
			else
				verityResult.IsOk = true;

			return verityResult;
		}

		/// <summary>
		/// 自訂回傳指定回應訊息的動作結果
		/// </summary>
		/// <param name="Result"></param>
		/// <param name="CustomHttpStatusCode"></param>
		/// <returns></returns>
		[ApiExplorerSettings(IgnoreApi = true)]
		public ResponseMessageResult ReturnResponseMessageResult(VerityResult Result, HttpStatusCode CustomHttpStatusCode)
		{
			return new ResponseMessageResult(
				Request.CreateResponse(CustomHttpStatusCode, Result)
			);
		}
	}
}
