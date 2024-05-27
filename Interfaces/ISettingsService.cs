using Luxa.Data;
using Luxa.Models;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Luxa.Interfaces
{
	public interface ISettingsService
	{
		//Task<bool> SetNewPassword(UserModel user, string oldPassword, string newPassword);
		//Task<bool> ChangeEmail(UserModel userModel, string newEmail);
		Task<string> ChangePassword(UserModel? user, string oldPassword, string newPassword);
		DataChangeVM? GetDataChangeVMFromUser(UserModel? user);
		Task<string> ChangeData(UserModel? user, bool isValid, DataChangeVM dataChangeVM);
	}
}

