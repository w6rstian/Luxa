using Luxa.Data;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext context;
        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
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
            var users = context.Users.ToList();
            



            return View(users);
        }

    }
}
