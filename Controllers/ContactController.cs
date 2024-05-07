using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult UserContact()
        {
            return View();
        }
    }
}
