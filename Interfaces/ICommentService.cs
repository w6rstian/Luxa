using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentModel>> GetAllComments();
        Task<IEnumerable<CommentModel>> GetCommentsForPhoto(int photoId);
        Task AddComment(string comment, int photoId, UserModel user);
        Task RemoveComment(int id);
    }
}
