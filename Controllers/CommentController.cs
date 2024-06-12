using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Mvc;

namespace Luxa.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                _commentService.AddComment(comment);
            }
            return RedirectToAction("PhotoDetails", new { id = comment.PhotoId });
        }

        [HttpPost]
        public ActionResult DeleteComment(int id, int photoId)
        {
            _commentService.DeleteComment(id);
            return RedirectToAction("PhotoDetails", new { id = photoId });
        }

        public ActionResult GetComments(int photoId)
        {
            var comments = _commentService.GetComments(photoId);
            return PartialView("_CommentsPartial", comments);
        }
    }
}
