using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Luxa.Services
{
	public class UserService : IUserService
	{

		private readonly SignInManager<UserModel> _signInManager;
		private readonly UserManager<UserModel> _userManager;

		public UserService(SignInManager<UserModel> signInManager,
			UserManager<UserModel> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}


		public UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user)
		{
			var currentUserTask = _userManager.GetUserAsync(user);
			currentUserTask.Wait();
			return currentUserTask.Result;
		}

		public async Task<bool> SaveUser(UserModel userModel)
		{
			var result = await _userManager.UpdateAsync(userModel);
			return result.Succeeded;
		}
	}
}
