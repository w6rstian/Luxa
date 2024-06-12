using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Luxa.Services
{
    public class UserService : IUserService
    {

        private readonly SignInManager<UserModel> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IPhotoRepository _photoRepository;

        public UserService(SignInManager<UserModel> signInManager,
            UserManager<UserModel> userManager, IPhotoRepository photoRepository, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _photoRepository = photoRepository;
            _context = context;
        }

        public UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user)
        {
            var currentUserTask = _userManager.GetUserAsync(user);
            currentUserTask.Wait();
            return currentUserTask.Result;
        }

        public UserModel? GetCurrentLoggedInUserWithPhotos(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);
            var currentUser = _context.Users
                .Include(u => u.Photos)
                .FirstOrDefault(u => u.Id == userId);
            return currentUser;
        }

        public async Task<UserModel?> GetUserByUserName(string userName) 
            => await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

        public async Task<bool> IsUserWithUserNameExist(string userName)
            => await _context.Users.AnyAsync(u => u.UserName == userName);

        public async Task<bool> SaveUser(UserModel userModel)
        {
            var result = await _userManager.UpdateAsync(userModel);
            return result.Succeeded;
        }

        public async Task<bool> UpdateReputation(UserModel userModel)
        {
            int reputation = 0;
            foreach (var photo in userModel.Photos)
            {
                _photoRepository.LikeCount(photo);
                reputation += photo.LikeCount;
            }
            userModel.Reputation = reputation;
            return await SaveUser(userModel);
        }
    }
}
