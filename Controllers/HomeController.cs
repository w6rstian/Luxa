using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Luxa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IPhotoService photoService, IHomeService homeService)
        {
            _logger = logger;
            _photoService = photoService;
            _homeService = homeService;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Landing()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Discover(string? tag) 
        {
            ViewBag.SelectItemListOrderBy = _homeService.GetOrderBySelectListItem();

            return View();
        }
    }
}
