using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Luxa.Controllers
{
	public class SettingsController : Controller
	{
		private readonly ISettingsService _settingsService;
		private readonly IUserService _userService;
		public SettingsController(ISettingsService settingsService, IUserService userService)
		{
			_settingsService = settingsService;
			_userService = userService;
		}
		public IActionResult Options()
		{
			return View();
		}
		public IActionResult ChangePassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(PasswordChangeVM passwordChange)
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			ViewData["Message"] = await _settingsService.ChangePassword(user, passwordChange.OldPassword, passwordChange.NewPassword);
			return View();
		}
		[HttpGet]
		public IActionResult ChangeData()
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			var result = _settingsService.GetDataChangeVMFromUser(user);
			return result != null ? View(result) : RedirectToAction("Error", "Home");
		}
		[HttpPost]
		public async Task<IActionResult> ChangeData(DataChangeVM dataChangeVM) 
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			ViewData["Message"] = await _settingsService.ChangeData(user,ModelState.IsValid,dataChangeVM);
			return View(dataChangeVM);
		}
		
	}
}
