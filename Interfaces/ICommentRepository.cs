using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface ICommentRepository
    {
        void AddComment(CommentModel comment);
        void DeleteComment(int id);
        IEnumerable<CommentModel> GetCommentsByPhotoId(int photoId);
    }
}
