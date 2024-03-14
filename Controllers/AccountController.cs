using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
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
    }
}
