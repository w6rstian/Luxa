using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;



        public CommentService(ICommentRepository commentRepository, IPhotoService photoService, IUserService userService)
        {
            _commentRepository = commentRepository;
            _photoService = photoService;
            _userService = userService;
        }

        public async Task<IEnumerable<CommentModel>> GetAllComments()
        {
            return await _commentRepository.GetAllComments();
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsForPhoto(int photoId)
        {
            return await _commentRepository.GetCommentsForPhoto(photoId);
        }

        public async Task AddComment(string comment, int photoId, UserModel user)
        {
            var photo = await _photoService.GetImageByIdAsync(photoId);

            CommentModel commentModel = new CommentModel() 
            {
                Comment = comment, Photo = photo, Owner = user
            };
            await _commentRepository.AddComment(commentModel);
        }

        public async Task RemoveComment(int id)
        {
            await _commentRepository.RemoveComment(id);
        }
    }
}
