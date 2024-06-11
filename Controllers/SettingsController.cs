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
            if (result != null)
            {
                result.Countries = new List<string>
				{
					"Polska",
					"Niemcy",
					"Francja",
					"Stany Zjednoczone",
					"Włochy",
					"Hiszpania",
					"Japonia",
					"Wielka Brytania",
					"Kanada",
					"Australia",
					"Rosja",
					"Chiny",
					"Indie",
					"Brazylia",
					"Meksyk",
				};
                return View(result);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
		public async Task<IActionResult> ChangeData(DataChangeVM dataChangeVM) 
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			ViewData["Message"] = await _settingsService.ChangeData(user,ModelState.IsValid,dataChangeVM);
			return View(dataChangeVM);
		}
        [HttpGet]
        public IActionResult ChangePrivacy()
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            var result = new PrivacyChangeVM { IsPrivate = user?.IsPrivate ?? false };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePrivacy(PrivacyChangeVM privacyChangeVM)
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            ViewData["Message"] = await _settingsService.ChangePrivacy(user, privacyChangeVM.IsPrivate);
            var result = new PrivacyChangeVM { IsPrivate = privacyChangeVM.IsPrivate };
            return View(result);
        }
    }
}
