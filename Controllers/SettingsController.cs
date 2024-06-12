using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Blazor;
using System.Drawing.Printing;

namespace Luxa.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserService _userService;
        private readonly UserManager<UserModel> _userManager;
        public SettingsController(ISettingsService settingsService, IUserService userService, UserManager<UserModel> userManager)
        {
            _settingsService = settingsService;
            _userService = userService;
            _userManager = userManager;
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
            ViewData["Message"] = await _settingsService.ChangeData(user, ModelState.IsValid, dataChangeVM);
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
        //zmiana opisu profilu
        [HttpGet]
        public IActionResult ChangeProfile()
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            var result = new ProfileChangeVM { Description = user.Description };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeProfile(ProfileChangeVM profileChangeVM)
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (profileChangeVM.Avatar != null && profileChangeVM.Avatar.Length > 0)
            {
                var avatarFileName = Path.GetFileName(profileChangeVM.Avatar.FileName);
                var avatarFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", avatarFileName);

                using (var stream = new FileStream(avatarFilePath, FileMode.Create))
                {
                    await profileChangeVM.Avatar.CopyToAsync(stream);
                }

                user.AvatarUrl = $"/avatars/{avatarFileName}";
            }

            if (profileChangeVM.Background != null && profileChangeVM.Background.Length > 0)
            {
                var backgroundFileName = Path.GetFileName(profileChangeVM.Background.FileName);
                var backgroundFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", backgroundFileName);

                using (var stream = new FileStream(backgroundFilePath, FileMode.Create))
                {
                    await profileChangeVM.Background.CopyToAsync(stream);
                }

                user.BackgroundUrl = $"/avatars/{backgroundFileName}";
            }

            user.Description = profileChangeVM.Description;
            await _userManager.UpdateAsync(user);

            ViewData["Message"] = "Profil zaaktualizowany pomyślnie";
            return View(profileChangeVM);
        }
    }
}
