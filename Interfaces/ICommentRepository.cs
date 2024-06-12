using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentModel>> GetAllComments();
        Task<IEnumerable<CommentModel>> GetCommentsForPhoto(int photoId);
        Task<CommentModel> GetCommentById(int id);
        Task AddComment(CommentModel comment);
        Task RemoveComment(int id);
    }
}
