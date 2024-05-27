using Luxa.Interfaces;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Mvc;

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
		[HttpGet]
		public IActionResult ChangeData()
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			if (user != null)
			{
				var dataChangeVM = new DataChangeVM
				{
					FirstName = user.FirstName,
					LastName = user.LastName,
					Country = user.Country,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
				};
				return View(dataChangeVM);
			}
			return RedirectToAction("Error","Home");

		}
		[HttpPost]
		public async Task<IActionResult> ChangeData(DataChangeVM dataChangeVM) 
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			if (ModelState.IsValid && user != null && dataChangeVM.Email != null)
			{
				string emailNotification;
				user.FirstName=dataChangeVM.FirstName;
				user.LastName=dataChangeVM.LastName;
				user.Country=dataChangeVM.Country;
				user.PhoneNumber=dataChangeVM.PhoneNumber;
				emailNotification = user.Email!=null && !user.Email.Equals(dataChangeVM.Email, StringComparison.OrdinalIgnoreCase) && await _settingsService.ChangeEmail(user, dataChangeVM.Email)
					? "Zmieniono adres E-mail\n"
					: "";


				if (await _userService.SaveUser(user))
				{
					ViewData["Message"] = "Zmiana danych powiodła się"+ emailNotification;
				}
			}
			else 
			{
				ViewData["Message"] = "Coś poszło nie tak";
			}

			return View(dataChangeVM);
		}
	}
}
