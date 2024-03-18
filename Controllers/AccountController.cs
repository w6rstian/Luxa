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
		public IActionResult UsersList()
		{
			var users = _context.Users.ToList();
			return View(users);
		}

		public IActionResult CreateUser()
		{
			return View();
		}

		[HttpPost]
		public IActionResult CreateUser(UserModel userData)
		{
			if (ModelState.IsValid)
			{
				/*var user = new UserModel()
				{
					FirstName = ,
					LastName = ,
					Nickname = ,
					Password = ,
					Email = ,
					Country = ,
					PhoneNumber = ,
					Category = ,

				}*/
			}

			return View();
		}

	}
}
