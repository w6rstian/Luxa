using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Luxa.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _notificationService;
        private readonly IUserService _userService;

        public AccountController(ApplicationDbContext context, SignInManager<UserModel> signInManager,
            UserManager<UserModel> userManager, NotificationService notificationService, IUserService userService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _notificationService = notificationService;
            _userService = userService;
        }

        //Atrybut do routingu (reszta kodu w program.cs)
        [Route("signin", Name = "SignIn")]
        [HttpGet]
        public IActionResult SignIn()
        {
            var model = new SignInVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM signInVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(signInVM.UserName!, signInVM.Password!, signInVM.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = _userService.GetCurrentLoggedInUserWithPhotos(User);
                    if (user != null)
                    {
                        await _userService.UpdateReputation(user);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Niepoprawna próba logowania");
            }

            return View(signInVM);
        }

        //google
        [HttpGet]
        public IActionResult SignInGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(SignIn));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = new UserModel
            {
                UserName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                //nadanie roli uzytkownikowi
                var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Regular);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("SignIn");
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet]
        [Route("signup", Name = "SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM signUpVM)
        {
            if (ModelState.IsValid)
            {
                UserModel user = new()
                {
                    UserName = signUpVM.UserName,
                    Email = signUpVM.Email,
                };

                var resultAddUser = await _userManager.CreateAsync(user, signUpVM.Password!);
                if (resultAddUser.Errors.Any())
                {
                    foreach (var error in resultAddUser.Errors)
                    {
                        switch (error.Code)
                        {
                            case "PasswordTooShort":
                                ModelState.AddModelError("", "Hasło musi zawierać minimum 6 znaków");
                                break;
                            case "DuplicateUserName":
                                ModelState.AddModelError("", "Użytkownik o danej nazwie użytkownika już istnieje, wybierz inną nazwę");
                                break;
                            default:
                                ModelState.AddModelError("", $"Nastąpił błąd {error.Code}, spróbuj ponownie");
                                break;
                        }
                    }
                    return View(signUpVM);
                }
                var resultAddRole = await _userManager.AddToRoleAsync(user, UserRoles.Regular);
                foreach (var error in resultAddRole.Errors)
                {
                    ModelState.AddModelError("", $"Nastąpił błąd {error.Code}, spróbuj ponownie");
                }
                if (resultAddRole.Errors.Any())
                {
                    return View(signUpVM);
                }
                if (resultAddUser.Succeeded && resultAddRole.Succeeded)
                {
                    var notifications = _context.Notifications.ToList();
                    user.UserNotifiacations.Add(new UserNotificationModel
                    { User = user, Notification = notifications[0] });
                    user.UserNotifiacations.Add(new UserNotificationModel
                    { User = user, Notification = notifications[1] });
                    _context.SaveChanges();
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(signUpVM);
        }
		[Authorize]
		public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            if (HttpContext.Request.Cookies[".AspNetCore.Session"] != null)
            {
                Response.Cookies.Delete(".AspNetCore.Session");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> UsersList()
        {
            var users = _context.Users.ToList();
            var usersWithRoles = new List<UsersListVM>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var notifications = _notificationService.GetNotificationsForUser(user.Id);

                var userListVM = new UsersListVM
                {
                    User = user,
                    Roles = roles,
                    Notifications = notifications
                };
                usersWithRoles.Add(userListVM);
            }

            return View(usersWithRoles);
        }

        //W fazie rozwoju
        [Authorize(Roles = "admin,moderator")]
        public IActionResult CreateUser()
        {
            return View();
        }
		//W fazie rozwoju
		[Authorize(Roles = "admin,moderator")]
		[HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM createUserVM)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel()
                {
                    FirstName = createUserVM.FirstName,
                    LastName = createUserVM.LastName,
                    UserName = createUserVM.Username,
                    Email = createUserVM.Email,
                    Country = createUserVM.Country,
                    PhoneNumber = createUserVM.PhoneNumber,
                };
                var resultAddUser = await _userManager.CreateAsync(user, createUserVM.Password!);
                var resultAddRole = await _userManager.AddToRoleAsync(user, createUserVM.Role);
                if (resultAddUser.Succeeded && resultAddRole.Succeeded)
                {
                    TempData["successMessage"] = "Utworzono użytkownika";
                    return RedirectToAction("UsersList");
                }

                foreach (var error in resultAddUser.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                foreach (var error in resultAddRole.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                TempData["successMessage"] = "Utworzono użytkownika";
                return RedirectToAction("UsersList");
            }
            else
            {
                TempData["errorMessage"] = "Wystąpił niezidentyfikowany błąd";
            }

            return View();
        }
		[Authorize(Roles = "admin,moderator")]
		[HttpPost]
        //[Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            await _userService.RemoveUserById(Id);
			return Ok();
		}
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return RedirectToAction("Error", "Home");
            EditUserVM editUserVM = new()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                PhoneNumber = user.PhoneNumber,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(editUserVM);
        }
		[Authorize(Roles = "admin,moderator")]
		[HttpPost]
        public async Task<IActionResult> EditUser(string Id, EditUserVM editUserVM)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = editUserVM.FirstName;
            user.LastName = editUserVM.LastName;
            user.Country = editUserVM.Country;
            if (await _userService.SaveUser(user))
                return RedirectToAction("UsersList", "Account");
            else
                return View(editUserVM);

        }
        [Authorize]
        public IActionResult UserNotifications()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if(userId==null)
            //	return RedirectToAction("SignIn");
            var user = _userService.GetCurrentLoggedInUser(User);
            if (user == null)
                return RedirectToAction("SignIn");
            var notifications = _notificationService.GetNotificationsForUser(user.Id);
            var userNotificationsVM = new UserNotificationsVM
            {
                User = user,
                Notifications = notifications
            };
            return View(userNotificationsVM);
        }
        [Authorize]
        [HttpPost]
        [Route("Account/UserNotifications/{notificationId}")]
        public IActionResult UserNotifications(int notificationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = _context.UserNotifications.FirstOrDefault(un => un.UserId == userId && un.NotificationId == notificationId);
            if (notification != null)
            {
                notification.IsViewed = true;
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
		[Authorize]
		public async Task<IActionResult> LoadMorePhotosToProfile(int pageNumber, int pageSize, string userName)
        {
            return ViewComponent("ProfilePhoto", new { pageNumber, pageSize, userName });
        }

        //profil avatar i tlo
        [Authorize]
        //[HttpGet]
        public async Task<IActionResult> UserProfile(string userName)
        {
            // zalogowany uzytkownik
            var currentUser = _userService.GetCurrentLoggedInUser(User);

            // czy istnieje
            if (await _userService.IsUserWithUserNameExist(userName) && currentUser != null && currentUser.UserName != null)
            {
                var profileUser = await _userService.GetUserByUserName(userName);

                if (profileUser == null || profileUser.UserName == null)
                {
                    return NotFound();
                }

                // domyslny avatar
                var avatarUrl = !string.IsNullOrEmpty(profileUser.AvatarUrl) ? profileUser.AvatarUrl : "/assets/blank-profile-picture.png";
                // domyslny avatar
                var backgroundUrl = !string.IsNullOrEmpty(profileUser.BackgroundUrl) ? profileUser.BackgroundUrl : "/assets/prostokat.png";

                var model = new UserProfileVM
                {
                    UserName = profileUser.UserName,
                    AvatarUrl = avatarUrl,
                    BackgroundUrl = backgroundUrl,
                    Description = profileUser.Description,
                    IsCurrentUserProfile = currentUser.UserName == userName,
                    IsFollowing = await _userService.IsFollowing(currentUser.Id, profileUser.Id),
                    PendingFollowRequests = currentUser.UserName == userName
                        ? await _userService.GetPendingFollowRequests(currentUser.Id)
                        : new List<FollowModel>()
                };

                return View(model);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = Path.GetFileName(avatar.FileName);
                var filePath = Path.Combine("wwwroot/avatars", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                user.AvatarUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                return RedirectToAction("UserProfile", new { userName = user.UserName });
            }

            return RedirectToAction("UserProfile", new { userName = User.Identity.Name });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadBackground(IFormFile background)
        {
            if (background != null && background.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var fileName = Path.GetFileName(background.FileName);
                var filePath = Path.Combine("wwwroot/avatars", fileName); ;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await background.CopyToAsync(stream);
                }

                user.BackgroundUrl = $"/avatars/{fileName}";
                await _userManager.UpdateAsync(user);

                return RedirectToAction("UserProfile", new { userName = user.UserName });
            }

            return RedirectToAction("UserProfile", new { userName = User.Identity.Name });
        }

        [Authorize]
        public async Task<IActionResult> Follow(string userName)
        {
            var currentUser = _userService.GetCurrentLoggedInUser(User);
            var followee = await _userService.GetUserByUserName(userName);

            if (followee == null || currentUser == null || currentUser.Id == followee.Id)
                return NotFound();

            var existingRequest = await _context.FollowRequests
                .FirstOrDefaultAsync(fr => fr.FollowerId == currentUser.Id && fr.FolloweeId == followee.Id);

            if (existingRequest == null)
            {
                var followRequest = new FollowModel
                {
                    FollowerId = currentUser.Id,
                    FolloweeId = followee.Id,
                    IsApproved = !followee.IsPrivate
                };

                _context.FollowRequests.Add(followRequest);
                await _context.SaveChangesAsync();

                if (followee.IsPrivate)
                {
                    await _notificationService.SendFollowRequestNotification(followee, currentUser);
                }
            }

            return RedirectToAction("UserProfile", new { userName });
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string userName)
        {
            var currentUser = _userService.GetCurrentLoggedInUser(User);
            var followee = await _userService.GetUserByUserName(userName);

            if (followee == null || currentUser == null || currentUser.Id == followee.Id)
                return NotFound();

            var followRequest = await _context.FollowRequests
                .FirstOrDefaultAsync(fr => fr.FollowerId == currentUser.Id && fr.FolloweeId == followee.Id);

            if (followRequest != null)
            {
                _context.FollowRequests.Remove(followRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserProfile", new { userName });
        }
    }

}
