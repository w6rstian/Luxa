using Luxa.Data;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<UserModel> signInManager;
		private readonly UserManager<UserModel> userManager;
		private readonly ApplicationDbContext _context;
		public AccountController(ApplicationDbContext context, SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
		{
			_context = context;
			this.signInManager = signInManager;
			this.userManager = userManager;
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
				var result = await signInManager.PasswordSignInAsync(model.UserName!, model.Password!, model.RememberMe, false);
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

				var resultAddUser = await userManager.CreateAsync(user, model.Password!);
				var resultAddRole = await userManager.AddToRoleAsync(user, UserRoles.Regular);
				if (resultAddUser.Succeeded && resultAddRole.Succeeded)
				{
					var notifications = _context.Notifications.ToList();
					user.UserNotifiacations.Add(new UserNotificationModel { User = user, Notification = notifications[0] });
					user.UserNotifiacations.Add(new UserNotificationModel { User = user, Notification = notifications[1] });
					_context.SaveChanges();
					await signInManager.SignInAsync(user, false);
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
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}


		//W fazie rozwoju
		public IQueryable<NotificationModel> GetNotificationsForUser(string userId) 
			=> _context.Users
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserNotifiacations) 
				.Select(un => un.Notification); 
		
		[Authorize(Roles = "admin,moderator")]
		public async Task<IActionResult> UsersList()
		{
			var users = _context.Users.ToList();
			var usersWithRoles = new List<UsersListVM>();
			foreach (var user in users)
			{
				var roles = await userManager.GetRolesAsync(user);
				var notifications = GetNotificationsForUser(user.Id);

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
				var resultAddUser = await userManager.CreateAsync(user, userData.Password!);
				var resultAddRole = await userManager.AddToRoleAsync(user, userData.Role);
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


	}
}
