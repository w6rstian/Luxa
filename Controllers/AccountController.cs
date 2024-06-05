using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInVM signInVM)
		{
			if (ModelState.IsValid)
			{
				var result =
					await _signInManager.PasswordSignInAsync(signInVM.UserName!, signInVM.Password!, signInVM.RememberMe, false);
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
				return View(signInVM);
			}

			return View(signInVM);
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
				var resultAddRole = await _userManager.AddToRoleAsync(user, UserRoles.Regular);
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

				foreach (var error in resultAddUser.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				foreach (var error in resultAddRole.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				return View(signUpVM);
			}

			return View(signUpVM);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			HttpContext.Session.Clear();
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
		public IActionResult DeleteUser(string Id)
		{
			return View();
		}
		[Authorize(Roles = "admin,moderator")]
		public async Task<IActionResult> EditUser(string Id)
		{
			var user = await _userManager.FindByIdAsync(Id);
			if (user == null)
				return RedirectToAction("Error", "Home");
			EditUserVM editUserVM = new EditUserVM()
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
		[HttpPost]
		public IActionResult EditUser(EditUserVM editUserVM)
		{
			return View();
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


		//Do tworzenia powiadomień ale jeszcze nic z tym nie robiłem
		public IActionResult AdminNotifications()
		{
			return View();
		}

	}

}
