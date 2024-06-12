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
            user.Description = profileChangeVM.Description;
            await _userManager.UpdateAsync(user);
            ViewData["Message"] = "Profile updated successfully!";
            return View(profileChangeVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = Path.GetFileName(avatar.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                user.AvatarUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                return RedirectToAction("ChangeProfile");
            }

            return RedirectToAction("ChangeProfile");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadBackground(IFormFile background)
        {
            if (background != null && background.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = Path.GetFileName(background.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await background.CopyToAsync(stream);
                }

                user.BackgroundUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                return RedirectToAction("ChangeProfile");
            }

            return RedirectToAction("ChangeProfile");
        }
    }
}
