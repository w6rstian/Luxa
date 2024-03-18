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
