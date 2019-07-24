using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SamsApi.Models;
using Services.Interface;
using Services.Service;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Entities;

namespace SamsApi.BLL
{
	/// <summary>
	/// 授權商業邏輯層
	/// </summary>
	public class AuthBLL: IDisposable
	{
		private ApplicationDbContext _ctx;
		private UserManager<ApplicationUser> _userManager;

		UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(new ApplicationDbContext());

		readonly IAspNetUsersService _aspnetUsersService;

		/// <summary>
		/// 授權商業邏輯層
		/// </summary>
		public AuthBLL()
		{
			_ctx = new ApplicationDbContext();
			_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
			_userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
			{
				AllowOnlyAlphanumericUserNames = false
			};

			_aspnetUsersService = new AspNetUsersService();
		}

		/// <summary>
		/// 依帳號、密碼找出使用者
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<ApplicationUser> FindUser(string userName, string password)
		{
			return await _userManager.FindAsync(userName, password);
		}

		/// <summary>
		/// 搜尋Client
		/// </summary>
		/// <param name="clientId"></param>
		/// <returns></returns>
		public Client FindClient(string clientId)
		{
			return _ctx.Clients.Find(clientId);
		}

		/// <summary>
		/// 建立RefreshToken
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public async Task<bool> AddRefreshToken(RefreshToken token)
		{
			var existingToken = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

			//如果存到資料表時,Token已存在則先移除舊的Token
			if (existingToken != null)
			{
				foreach (var _existingToken in existingToken)
				{
					//var result = await RemoveRefreshToken(_existingToken);
					_ctx.RefreshTokens.Remove(_existingToken);
				}
			}

			_ctx.RefreshTokens.Add(token);
			return await _ctx.SaveChangesAsync() > 0;
		}

		/// <summary>
		/// 依帳號找出使用者
		/// </summary>
		/// <param name="Username"></param>
		/// <returns></returns>
		public async Task<ApplicationUser> FindByName(string Username)
		{
			return await _userManager.FindByNameAsync(Username);
		}

		public void Dispose()
		{
			_ctx.Dispose();
			_userManager.Dispose();
		}
	}
}