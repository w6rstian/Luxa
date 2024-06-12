using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddComment(CommentModel comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }

        public IEnumerable<CommentModel> GetCommentsByPhotoId(int photoId)
        {
            return _context.Comments.Where(c => c.PhotoId == photoId).ToList();
        }
    }

}
