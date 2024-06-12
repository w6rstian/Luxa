using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void AddComment(CommentModel comment)
        {
            _commentRepository.AddComment(comment);
        }

        public void DeleteComment(int id)
        {
            _commentRepository.DeleteComment(id);
        }

        public IEnumerable<CommentModel> GetComments(int photoId)
        {
            return _commentRepository.GetCommentsByPhotoId(photoId);
        }
    }
}
