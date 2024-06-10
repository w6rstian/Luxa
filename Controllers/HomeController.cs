using Luxa.Interfaces;
using Luxa.Models;
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
		private readonly IUserService _userService;

		public HomeController(ILogger<HomeController> logger, IPhotoService photoService, IHomeService homeService, IUserService userService)
		{
			_logger = logger;
			_photoService = photoService;
			_homeService = homeService;
			_userService = userService;
		}

		//Widoki
		[Authorize]
		public IActionResult Index()
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
			ViewBag.SelectItemListCategory = _homeService.GetCategoriesSelectListItem();
			ViewBag.Tag = tag;
			return View();
		}



		//Ajax, Js
		public async Task<IActionResult> LoadPhotosForDiscover(int pageNumber,
			int pageSize,
			string? tag,
			string? category,
			bool order,
			string? sortBy)
		{
			var user = _userService.GetCurrentLoggedInUser(User);
			if (user == null)
				//To jest do zmiany i przemyœlenia
				throw new NotImplementedException();
			var photos = await _photoService.GetPhotosWithIsLikedForDiscoverAsync(pageNumber, pageSize, user, tag, category, order, sortBy);

			/*
			foreach (var photo in photos)
			{
				var sessionKey = $"viewed_photo_{photo.Photo.Id}";
				if (HttpContext.Session.GetString(sessionKey) == null)
					HttpContext.Session.SetString(sessionKey, "viewed");
			}*/
			return Json(photos);
		}

	}
}
