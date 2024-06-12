using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommentModel>> GetAllComments()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsForPhoto(int photoId)
        {
            return await _context.Comments
                .Where(c => c.PhotoId == photoId)
                .ToListAsync();
        }

        public async Task<CommentModel> GetCommentById(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task AddComment(CommentModel comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
