using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ResourceLibrary;
using SamsApi.BLL;
using SamsApi.Models;
using Serilog;
using Services.Interface;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Utility;
using ViewModel.Entities;
using ViewModel.Enum;

namespace SamsApi.Providers
{
	public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
		private ILogger _logger = Log.Logger;
		readonly IAspNetUsersService _aspnetUsersService;
		readonly IMobilePushKeyService _mobilePushKeyService;

		public ApplicationOAuthProvider()
        {
			_aspnetUsersService = new AspNetUsersService();
			_mobilePushKeyService = new MobilePushKeyService();
		}

		/// <summary>
		/// 當 Token 端點的要求與 "password" 的 "grant_type" 抵達時呼叫。 
		/// 這會在使用者直接提供名稱和密碼認證到用戶端應用程式的使用者介面時發生，且用戶端應用程式會使用其來取得 "access_token" 和選擇性的 "refresh_token"。
		/// 若 Web 應用程式支援資源擁有者認證授與類型，其必須適當驗證 context.Username 和 context.Password。 若要簽發存取語彙基元，context.Validated 必須以新票證呼叫，此票證包括應該與存取票證相關的資源擁有者相關的宣告。 預設行為是要拒絕此授與類型。 
		/// 另請參閱 http://tools.ietf.org/html/rfc6749#section-4.30.2
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
			_logger.Information("SimpleAuthorizationServerProvider GrantResourceOwnerCredentials : 1");

			var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

			if (allowedOrigin == null) allowedOrigin = "*";
			_logger.Information("SimpleAuthorizationServerProvider GrantResourceOwnerCredentials : 2");

			//如果allowedOrigin被允許,則將允許來源加入Owin的context中,如果API來源不同的話則回應405狀態
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

			//檢查用戶名/密碼,
			string _userid = null;
			bool isPhoneNumberConfirmed;
			using (AuthBLL _auth = new AuthBLL())
			{
				IdentityUser user = await _auth.FindUser(context.UserName, context.Password);

				if (user == null)
				{
					context.SetError("invalid_grant", Resource.AccountOrPasswordError, "461");
					return;
				}
				_userid = user.Id;
				isPhoneNumberConfirmed = user.PhoneNumberConfirmed;
			}

			_logger.Information("SimpleAuthorizationServerProvider GrantResourceOwnerCredentials : 3");

			bool resetpasswd = false;

			var findUser = await _aspnetUsersService.QueryUsersByIDAsync(_userid);
			resetpasswd = findUser.resetPW == null ? false : Convert.ToBoolean(findUser.resetPW);
			_logger.Debug("resetpasswd：" + resetpasswd);

			//如果帳密有效的則產生claims-包含ClientId和UserName等等ticket資訊
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
			identity.AddClaim(new Claim("sub", context.UserName));
			identity.AddClaim(new Claim("role", "user"));

			var props = new AuthenticationProperties(new Dictionary<string, string>
				{
					{
						"as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
					},
					{
						"userName", context.UserName
					},
					{
						"resetPassword", resetpasswd.ToString() //帳號重設密碼登入(true/false)
                    },
					{
						"as:userId",_userid
					}
				});

			#region 加進cookiesIdentity 及  Claim：PasswordHash 與 KeyNumber
			var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
			ApplicationUser applicationUser = await userManager.FindAsync(context.UserName, context.Password);
			ClaimsIdentity cookiesIdentity = await applicationUser.GenerateUserIdentityAsync(userManager,
				CookieAuthenticationDefaults.AuthenticationType);

			identity.AddClaim(new Claim("PasswordHash", applicationUser.PasswordHash));

			string keyNumber = context.OwinContext.Get<string>("as:KeyNumber");
			identity.AddClaim(new Claim("KeyNumber", keyNumber));

			var ticket = new AuthenticationTicket(identity, props);
			context.Validated(ticket);
			context.Request.Context.Authentication.SignIn(cookiesIdentity);
			#endregion

			_logger.Information("SimpleAuthorizationServerProvider GrantResourceOwnerCredentials : END");
		}

		/// <summary>
		/// 在成功 Token 端點要求的最後階段呼叫。 
		/// 應用程式會實作此呼叫，以進行用來簽發存取或重新整理語彙基元的宣告最後修改。 
		/// 也可能使用此呼叫來新增其他回應參數至 Token 端點的 json 回應主體。
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			string userID = string.Empty;
			foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);

				if (property.Key == "as:userId")
					userID = property.Value;
			}

			string keyNumber = context.OwinContext.Get<string>("as:KeyNumber");
			string mobileType = context.OwinContext.Get<string>("as:MobileType");
			int intMobileType = 0;
			int.TryParse(mobileType, out intMobileType);

			int newClaimCount = 0;
			int isPhoneNumberConfirmedResetCodeCount = 0;
			var claimsIdentity = (ClaimsIdentity)context.Identity;
			IEnumerable<Claim> claims = claimsIdentity.Claims;
			foreach (var item in claims)
			{
				if (item.Type == "newClaim")
					newClaimCount++;
			}

			if (newClaimCount == 0)
				MobilePushKeyLogic(userID, keyNumber, intMobileType, false);

			if (isPhoneNumberConfirmedResetCodeCount == 1)
				MobilePushKeyLogic(userID, keyNumber, intMobileType, true);

			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// 產生更新Token
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
		{
			var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
			var currentClient = context.ClientId;

			if (originalClient != currentClient)
			{
				context.Rejected();
				Task.FromResult<object>(null);
			}

			// 更新登入ticket for 更新Token的要求
			var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
			newIdentity.AddClaim(new Claim("newClaim", "refreshToken"));

			var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
			context.Validated(newTicket);

			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// 驗證邏輯
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			#region 推播key Set
			context.OwinContext.Set<string>("as:KeyNumber", context.Parameters.Get("KeyNumber"));
			context.OwinContext.Set<string>("as:MobileType", context.Parameters.Get("MobileType"));
			#endregion

			#region  Client Logic
			string clientId = string.Empty;
			string clientSecret = string.Empty;
			Client client = null;

			//從header的authorization欄位中取得Base 64編碼後的Client id 和 secret,
			//另一方式為client_id/client_secret以x-www-form-urlencoded傳送
			if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
			{
				context.TryGetFormCredentials(out clientId, out clientSecret);
			}

			//Client端若未配置ClientId則回傳錯誤
			if (context.ClientId == null)
			{
				context.Validated();
				return Task.FromResult<object>(null);
			}

			//收到ClientId後檢查資料庫，如果ClientId未記錄在資料庫中將拒絕這個請求。
			using (AuthBLL _auth = new AuthBLL())
			{
				client = _auth.FindClient(context.ClientId);
			}

			if (client == null)
			{
				context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
				return Task.FromResult<object>(null);
			}

			//檢查客戶端的應用程式,
			//如果為Java Script則為非機密型應用程式,我們將不檢查或要求加密證書
			//如果是Native應用程式，就必須強制要求加密證書
			if (client.ApplicationType == ApplicationTypes.NativeConfidential)
			{
				if (string.IsNullOrWhiteSpace(clientSecret))
				{
					context.SetError("invalid_clientId", "Client secret should be sent.");
					return Task.FromResult<object>(null);
				}
				else
				{
					if (client.Secret != Helper.GetHash(clientSecret))
					{
						context.SetError("invalid_clientId", "Client secret is invalid.");
						return Task.FromResult<object>(null);
					}
				}
			}

			//檢查Client的存活期,如果不在存活期內則回應無效
			if (!client.Active)
			{
				context.SetError("invalid_clientId", "Client is inactive.");
				return Task.FromResult<object>(null);
			}

			//存儲的客戶端允許的生成
			context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);

			//更新Token生命時間值，生成新Token後設定到期時間
			context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
			#endregion

			context.Validated();
			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// 手機推播 key 資料處理邏輯 
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="KeyNumber"></param>
		/// <param name="MobileType"></param>
		/// <param name="IsFirst"></param>
		private void MobilePushKeyLogic(string UserID, string KeyNumber, int MobileType, bool IsFirst)
		{
			if (!string.IsNullOrEmpty(KeyNumber))
			{
				var queryByUserID = _mobilePushKeyService.GetByUserID(UserID);
				if (queryByUserID == null)
				{
					//新增
					_mobilePushKeyService.CreateOrUpdate(UserID, KeyNumber, MobileType);
				}
				else
				{
					//同一組不用做處理，若不同就更新
					var existMobilePushKey = _mobilePushKeyService.GetByParam(KeyNumber, UserID);
					if (existMobilePushKey == null)
					{
						_mobilePushKeyService.CleanKeyNumber(KeyNumber);
						_mobilePushKeyService.CreateOrUpdate(UserID, KeyNumber, MobileType);
					}
				}
			}
		}
    }
}