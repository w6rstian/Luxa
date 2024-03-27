using Luxa.Data;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Luxa.Services;


namespace Luxa.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<UserModel> _signInManager;
		private readonly UserManager<UserModel> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly NotificationService _notificationService;

		public AccountController(ApplicationDbContext context, SignInManager<UserModel> signInManager,
			UserManager<UserModel> userManager, NotificationService notificationService)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_notificationService = notificationService;
		}

		//Atrybut do routingu (reszta kodu w program.cs)
		[Route("signin", Name = "SignIn")]
		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInVM model)
		{
			if (ModelState.IsValid)
			{
				var result =
					await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("", "Niepoprawna próba logowania");
				return View(model);
			}

			return View(model);
		}

		[HttpGet]
		[Route("signup", Name = "SignUp")]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpVM model)
		{
			if (ModelState.IsValid)
			{
				UserModel user = new()
				{
					UserName = model.UserName,
					Email = model.Email,
				};

				var resultAddUser = await _userManager.CreateAsync(user, model.Password!);
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

				return View(model);
			}

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
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
		public async Task<IActionResult> CreateUser(CreateUserVM userData)
		{
			if (ModelState.IsValid)
			{
				var user = new UserModel()
				{
					FirstName = userData.FirstName,
					LastName = userData.LastName,
					UserName = userData.Username,
					Email = userData.Email,
					Country = userData.Country,
					PhoneNumber = userData.PhoneNumber,
				};
				var resultAddUser = await _userManager.CreateAsync(user, userData.Password!);
				var resultAddRole = await _userManager.AddToRoleAsync(user, userData.Role);
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

		public async Task<IActionResult> UserNotifications()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);
			var notifications = _notificationService.GetNotificationsForUser(userId);
			var userNotificationsVM = new UserNotificationsVM
			{
				User = user,
				Notifications = notifications
			};
			return View(userNotificationsVM);
		}
		[HttpPost]
		[Route("Account/UserNotifications/{notificationId}")]
		public IActionResult UserNotifications(int notificationId)
		{
			// Znajdź powiadomienie w bazie danych
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var notification = _context.UserNotifications.FirstOrDefault(un => un.UserId == userId && un.NotificationId == notificationId);
			if (notification != null)
			{
				// Zaktualizuj stan odczytania
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
