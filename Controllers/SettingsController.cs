using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Blazor;
using System.Drawing.Printing;

namespace Luxa.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserService _userService;
        private readonly UserManager<UserModel> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;
        public SettingsController(ISettingsService settingsService, IUserService userService, UserManager<UserModel> userManager,
            ApplicationDbContext context, NotificationService notificationService)
        {
            _settingsService = settingsService;
            _userService = userService;
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
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
                result.Countries = [.. CountryOptions.Countries];
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
            var result = new ProfileChangeVM { Description = user?.Description };
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDescription(ProfileChangeVM profileChangeVM)
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            if (user == null)
            {
                return Unauthorized();
            }

            user.Description = profileChangeVM.Description;
            await _userManager.UpdateAsync(user);

            ViewData["DescriptionMessage"] = "Profil zmodyfikowany pomyślnie!";
            return View("ChangeProfile", profileChangeVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile? avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }
                var fileName = Path.GetFileName(avatar.FileName);
                var filePath = Path.Combine("wwwroot/avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                user.AvatarUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                ViewData["AvatarMessage"] = "Profil zmodyfikowany pomyślnie!";
                return RedirectToAction("ChangeProfile");
            }

            ViewData["AvatarMessage"] = "Nie wybrano pliku.";
            return RedirectToAction("ChangeProfile");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadBackground(IFormFile? background)
        {
            if (background != null && background.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = Path.GetFileName(background.FileName);
                var filePath = Path.Combine("wwwroot/avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await background.CopyToAsync(stream);
                }

                user.BackgroundUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                ViewData["BackgroundMessage"] = "Profil zmodyfikowany pomyślnie!";
                return RedirectToAction("ChangeProfile");
            }

            ViewData["BackgroundMessage"] = "Nie wybrano pliku.";
            return RedirectToAction("ChangeProfile");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeFollows()
        {
            var user = _userService.GetCurrentLoggedInUser(User);
            var followRequests = await _userService.GetPendingFollowRequests(user.Id);
            var model = new FollowsChangeVM
            {
                PendingFollowRequests = followRequests
            };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ApproveFollow(int requestId)
        {
            var followRequest = await _context.FollowRequests
                .Include(fr => fr.Followee).Include(fr => fr.Follower)
                .FirstOrDefaultAsync(fr => fr.Id == requestId);

            var currentUser = _userService.GetCurrentLoggedInUser(User);

            if (followRequest == null || followRequest.FolloweeId != currentUser.Id)
                return NotFound();

            followRequest.IsApproved = true;
            await _context.SaveChangesAsync();

            // powiadomienie o potwierdzeniu prosby o obserwacje
            await _notificationService.SendFollowApprovedNotification(followRequest.Follower, currentUser);

            return RedirectToAction("ChangeFollows");
        }

        [Authorize]
        public async Task<IActionResult> RejectFollow(int requestId)
        {
            var followRequest = await _context.FollowRequests
                .Include(fr => fr.Followee)
                .FirstOrDefaultAsync(fr => fr.Id == requestId);

            var currentUser = _userService.GetCurrentLoggedInUser(User);

            if (followRequest == null || followRequest.FolloweeId != currentUser.Id)
                return NotFound();

            _context.FollowRequests.Remove(followRequest);
            await _context.SaveChangesAsync();

            // powiadomienie o odrzuceniu prosby o obserwacje
            await _notificationService.SendFollowRejectedNotification(followRequest.Follower, currentUser);

            return RedirectToAction("ChangeFollows");
        }
    }
}
