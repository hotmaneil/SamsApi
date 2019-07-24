using Newtonsoft.Json;
using Serilog;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Push;
using ViewModel.VerifyViewModel;

namespace Services.Service
{
	public class PushService: IPushService
	{
		private ILogger _logger = Log.Logger;

		#region Main function
		/// <summary>
		/// FCM (Google firebase colud Message)
		/// </summary>
		/// <param name="RegistrationIDList">手機裝置中已註冊的KeyNumber清單</param>
		/// <param name="PushMsg">播訊息</param>
		/// <returns></returns>
		private VerityResult AndroinPushByFCM(List<string> RegistrationIDList, AndroidPushMessageViewModel PushMsg)
		{
			//Google Api Key
			string apiKEY = ConfigurationManager.AppSettings["AndroidApiKey"].ToString();

			//Google firebase colud message SenderId
			string senderID = ConfigurationManager.AppSettings["FCMSenderId"].ToString();

			_logger.Information("PushService AndroinPushByFCM paras: apikey= {0}, senderid= {1}, RegistrationIDList= {2}, PushMsg= {3}"
				, apiKEY, senderID, JsonConvert.SerializeObject(RegistrationIDList), JsonConvert.SerializeObject(PushMsg));

			VerityResult result = new VerityResult();
			result.IsOk = true;

			try
			{
				if (RegistrationIDList != null && RegistrationIDList.Count > 0)
				{
					foreach (var regid in RegistrationIDList)
					{
						WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
						webRequest.Method = "post";
						webRequest.ContentType = "application/json";

						#region Set the Header property of the WebRequest.
						//"FCM伺服器金鑰";
						webRequest.Headers.Add(string.Format("Authorization: key={0}", apiKEY));

						//FCM寄件者 ID
						webRequest.Headers.Add(string.Format("Sender: id={0}", senderID));
						#endregion

						var data = new
						{
							to = regid,
							data = PushMsg
						};

						_logger.Information("PushService AndroinPushByFCM data:" + JsonConvert.SerializeObject(data));

						string json = JsonConvert.SerializeObject(data);
						Byte[] byteArray = Encoding.UTF8.GetBytes(json);
						webRequest.ContentLength = byteArray.Length;

						using (Stream dataStream = webRequest.GetRequestStream())
						{
							dataStream.Write(byteArray, 0, byteArray.Length);
							using (WebResponse webResponse = webRequest.GetResponse())
							{
								using (Stream dataStreamResponse = webResponse.GetResponseStream())
								{
									using (StreamReader tReader = new StreamReader(dataStreamResponse))
									{
										String sResponseFromServer = tReader.ReadToEnd();
										string str = sResponseFromServer;
										_logger.Information("PushService SendPushNotificationToFCM Result:" + str);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				result.IsOk = false;
				result.Message = ex.Message.ToString();
				_logger.Error(ex, "AndroinPushByFCM Error:" + ex.Message.ToString());
			}
			return result;
		}
		#endregion

		#region Android

		/// <summary>
		/// Android 推播訊息
		/// </summary>
		/// <param name="RegistrationIDList">手機裝置中已註冊的KeyNumber清單</param>
		/// <param name="PushMsg">推播訊息</param>
		/// <returns></returns>
		public async Task<VerityResult> AndroidPushMessage(List<string> RegistrationIDList, AndroidPushMessageViewModel PushMsg)
		{
			VerityResult result = new VerityResult();
			result = AndroinPushByFCM(RegistrationIDList, PushMsg);
			return await Task.Run(() => result);
		}
		#endregion
	}
}
