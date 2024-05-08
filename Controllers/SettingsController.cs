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
			var user = _settingsService.GetCurrentLoggedInUser(User);
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
			var user = _settingsService.GetCurrentLoggedInUser(User);
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
		[HttpPost]
		public async Task<IActionResult> ChangeData(DataChangeVM dataChangeVM) 
		{
			if (ModelState.IsValid)
			{
				var user = _settingsService.GetCurrentLoggedInUser(User);
				string emailNotification;
				user.FirstName=dataChangeVM.FirstName;
				user.LastName=dataChangeVM.LastName;
				user.Country=dataChangeVM.Country;
				user.PhoneNumber=dataChangeVM.PhoneNumber;
				if (user.Email!= dataChangeVM.Email.ToLower() & await _settingsService.ChangeEmail(user, dataChangeVM.Email))
					emailNotification = "Zmieniono adres E-mail\n";
				else
					emailNotification = "";


				if (await _settingsService.SaveUser(user))
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
