using Luxa.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Components
{
    public class ProfilePhotoViewComponent : ViewComponent
    {
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public ProfilePhotoViewComponent(IPhotoService photoService, IUserService userService)
        {
            _photoService = photoService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageNumber, int pageSize, string userName)
        {
            //obecnie zalogowany user
            var currentUser = _userService.GetCurrentLoggedInUser(UserClaimsPrincipal);
            //właściciel profilu
            var user = await _userService.GetUserByUserName(userName);
            //sprawdzenie dostępu do profilu (jeśli jest prywatny, zalogowany musi być właścicielem)
            if (user == null || currentUser == null || (user.IsPrivate && user.Id != currentUser.Id && !await _userService.IsFollowing(currentUser.Id, user.Id)))
                return Content(string.Empty);
            //pobieranie zdjęć z polubieniami oraz nazwą właściciela
            var photos = await _photoService.GetPhotosWithIsLikedForProfileAsync(pageNumber, pageSize, user);
            string Path = "/assets/";
            ViewBag.LikePath = Path+ "hand-thumbs-up-fill.svg";
            ViewBag.UnlikePath =Path+ "hand-thumbs-up.svg";
			return View(photos);

        }
    }
}
