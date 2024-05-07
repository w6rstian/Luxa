using Luxa.Interfaces;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
	public class SettingsController : Controller
	{
		private readonly ISettingsService _settingsService;
		public SettingsController(ISettingsService settingsService)
		{
			_settingsService = settingsService;
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
			var user = await _settingsService.GetCurrentLoggedInUser(User);
			if (user != null)
			{
				if (await _settingsService.SetNewPassword(user, passwordChange.OldPassword, passwordChange.NewPassword))
				{
					ViewData["Message"] = "Hasło zostało pomyślnie zmienione";
					return View();
				}
			}
			ViewData["Message"] = "Nie można zmienić hasła. Sprawdź poprawność wprowadzonych danych";
			return View();
		}
		public IActionResult ChangeData()
		{
			var user = _settingsService.GetCurrentLoggedInUser(User);
			var dataChangeVM = new DataChangeVM
			{
				FirstName = user.Result.FirstName,
				LastName = user.Result.LastName,
				Country = user.Result.Country,
				Email = user.Result.Email

			};
			return View(dataChangeVM);

		}
	}
}
