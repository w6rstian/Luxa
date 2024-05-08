using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Luxa.Services
{
	public class SettingsService : ISettingsService
	{



		private readonly SignInManager<UserModel> _signInManager;
		private readonly UserManager<UserModel> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly NotificationService _notificationService;

		public SettingsService(ApplicationDbContext context, SignInManager<UserModel> signInManager,
			UserManager<UserModel> userManager, NotificationService notificationService)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_notificationService = notificationService;
		}

		public async Task<bool> SetNewPassword(UserModel user, string oldPassword, string newPassword)
		{
			var result = await _signInManager.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
			return result.Succeeded;
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
		public async Task<bool> ChangeEmail(UserModel userModel, string newEmail)
		{
			var token = await _userManager.GenerateChangeEmailTokenAsync(userModel, newEmail);
			var result = await _userManager.ChangeEmailAsync(userModel, newEmail, token);
			return result.Succeeded;
		}
	}
}
