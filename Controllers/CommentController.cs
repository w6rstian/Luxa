using Microsoft.AspNetCore.Mvc;
using Luxa.Models;
using Luxa.Services;
using Luxa.Interfaces;
using Luxa.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Luxa.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPhotoService _photoService;
        private readonly ApplicationDbContext _context; //wstrzykiwanie kontekstu
        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService _userService;
    

        public CommentController(ICommentService commentService, IUserService userService, ApplicationDbContext context, UserManager<UserModel> userManager, IWebHostEnvironment hostEnvironment, IPhotoService photoService)
        {
            _commentService = commentService;
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _photoService = photoService;
            _userService = userService;
            

        }

        // Akcja pobierająca komentarze dla określonego zdjęcia
        public async Task<IActionResult> GetCommentsForPhoto(int photoId)
        {
            var comments = await _commentService.GetCommentsForPhoto(photoId);
            return PartialView("_CommentsPartial", comments);
        }

        // Akcja dodawania komentarza do zdjęcia
        [HttpPost]
        public async Task<IActionResult> AddComment(string Content, int photoId)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetCurrentLoggedInUser(User);
                await _commentService.AddComment(Content,photoId,user);
            }
            return RedirectToAction("Details", "Photos", new { id = photoId });
        }

        // Akcja usuwania komentarza z zdjęcia
        [HttpPost]
        public async Task<IActionResult> RemoveComment(int commentId, int photoId)
        {
            await _commentService.RemoveComment(commentId);
            return RedirectToAction("Details", "Photos", new { id = photoId });
        }
    }
}
