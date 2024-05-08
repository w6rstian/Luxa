using Luxa.Data;
using Luxa.Models;
using Luxa.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Luxa.Interfaces
{
	public interface ISettingsService
	{
		UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user);
		Task<bool> SetNewPassword(UserModel user, string oldPassword, string newPassword);
		Task<bool> SaveUser(UserModel userModel);
		Task<bool> ChangeEmail(UserModel userModel, string newEmail);
	}
}

