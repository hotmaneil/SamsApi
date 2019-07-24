using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using SamsApi.BLL;
using Services.Interface;
using Services.Service;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Utility;
using ViewModel.Entities;

namespace SamsApi.Providers
{
	public class RefreshTokenProvider: IAuthenticationTokenProvider
	{
		readonly IAspNetUsersService _aspnetUsersService;

		public RefreshTokenProvider()
		{
			_aspnetUsersService = new AspNetUsersService();
		}

		/// <summary>
		/// refreshTokens之安全執行緒集合
		/// </summary>
		private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens =
		new ConcurrentDictionary<string, AuthenticationTicket>();

		/// <summary>
		/// 建立 AuthenticationTokenCreateContext
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task CreateAsync(AuthenticationTokenCreateContext context)
		{
			var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

			if (string.IsNullOrEmpty(clientid))
			{
				return;
			}

			//產成的新Token的唯一標別碼，在此使用Guid(有需要再改其它演算法)
			var refreshTokenId = Guid.NewGuid().ToString("n");

			//更新Token的生存時間值，該值將被用於確定新Token有多長的有效期
			using (AuthBLL _auth = new AuthBLL())
			{
				var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

				//有值延長為2倍，若為null則預設60分鐘
				double doubleRefreshTokenLifeTime = 60;
				bool isDoubleRefreshTokenLifeTime = double.TryParse(refreshTokenLifeTime, out doubleRefreshTokenLifeTime);
				if (!isDoubleRefreshTokenLifeTime)
					doubleRefreshTokenLifeTime = 60;
				else
					doubleRefreshTokenLifeTime = doubleRefreshTokenLifeTime * 2;

				var token = new RefreshToken()
				{
					Id = Helper.GetHash(refreshTokenId),
					ClientId = clientid,
					Subject = context.Ticket.Identity.Name,
					IssuedUtc = DateTime.UtcNow,

					//有效期限(分鐘,有需要再改)
					ExpiresUtc = DateTime.UtcNow.AddMinutes(doubleRefreshTokenLifeTime)
				};

				context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
				context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

				//在資料庫中以序列化形式儲存
				token.ProtectedTicket = context.SerializeTicket();

				//儲存到RefreshTokens資料表中
				var result = await _auth.AddRefreshToken(token);

				if (result)
				{
					//建立暫存refreshToken用
					var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
					{
						IssuedUtc = context.Ticket.Properties.IssuedUtc,
						ExpiresUtc = DateTime.UtcNow.AddMinutes(doubleRefreshTokenLifeTime)
					};

					var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);
					_refreshTokens.TryAdd(refreshTokenId, refreshTokenTicket);

					//回傳新的Token
					context.SetToken(refreshTokenId);
				}
			}
		}

		/// <summary>
		/// 接收RefreshToken
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
		{
			AuthenticationTicket ticket;

			if (_refreshTokens.TryRemove(context.Token, out ticket))
			{
				var query = _aspnetUsersService.QueryUsersByPhoneNumber(ticket.Identity.Name);
				if (query.PhoneNumberConfirmed)
					context.SetTicket(ticket);
				else
					return;
			}
		}

		public void Create(AuthenticationTokenCreateContext context)
		{
			throw new NotImplementedException();
		}

		public void Receive(AuthenticationTokenReceiveContext context)
		{
			throw new NotImplementedException();
		}
	}
}