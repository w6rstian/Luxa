using Luxa.Data;
using Luxa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _context;
		public AccountController(ApplicationDbContext context)
		{
			_context = context;
		}
		//Atrybut do routingu (reszta kodu w program.cs)
		[Route("signin", Name = "SignIn")]
		public IActionResult SignIn()
		{
			return View();
		}

		[Route("signup", Name = "SignUp")]
		public IActionResult SignUp()
		{
			return View();
		}
		//W fazie rozwoju
		public IActionResult UsersList()
		{
			var users = _context.Users.ToList();
			return View(users);
		}
		//W fazie rozwoju
		public IActionResult CreateUser()
		{
			return View();
		}
		//W fazie rozwoju
		[HttpPost]
		public IActionResult CreateUser(UserModel userData)
		{
			if (ModelState.IsValid)
			{
				var user = new UserModel()
				{
					FirstName = userData.FirstName,
					LastName = userData.LastName,
					Nickname = userData.Nickname,
					Password = userData.Password,
					Email = userData.Email,
					Country = userData.Country,
					PhoneNumber = userData.PhoneNumber,
					Category = userData.Category
				};

				_context.Users.Add(user);
				_context.SaveChanges();
				TempData["successMessage"] = "Powiodło się";
				return RedirectToAction("UsersList");

			}
			else 
			{
				TempData["errorMessage"] = "Model data is not valid";

			}

			return View();
		}

	}
}
