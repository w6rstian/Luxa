using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface ICommentService
    {
        void AddComment(CommentModel comment);
        void DeleteComment(int id);
        IEnumerable<CommentModel> GetComments(int photoId);
    }
}
