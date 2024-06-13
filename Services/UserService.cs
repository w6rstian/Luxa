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

		public async Task<bool> RemoveUserById(string Id)
		{
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) 
            {
                return false;
            }
            var result = await _userManager.DeleteAsync(user);
			return result.Succeeded;
		}

		public async Task<bool> SaveUser(UserModel userModel)
        {
            var result = await _userManager.UpdateAsync(userModel);
            return result.Succeeded;
        }

        /*public int GetUserLevel(int reputation)
		{
			int[] thresholds = { 20, 50, 100, 200, 500, 1000, 5000 };
			for (int i = 1; i <= thresholds.Length; i++)
			{
				if (reputation < thresholds[i])
				{
					return i;
				}
			}
			return thresholds.Length + 1;

		}*/


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

        public async Task<bool> IsFollowing(string followerId, string followeeId)
        {
            return await _context.FollowRequests
                .AnyAsync(fr => fr.FollowerId == followerId && fr.FolloweeId == followeeId && fr.IsApproved);
        }

        public async Task<List<FollowModel>> GetPendingFollowRequests(string userId)
        {
            return await _context.FollowRequests
                .Where(fr => fr.FolloweeId == userId && !fr.IsApproved)
                .Include(fr => fr.Follower)
                .ToListAsync();
        }

        public async Task<List<UserModel>> GetFollowedUsers(string userId)
        {
            return await _context.FollowRequests
                .Where(fr => fr.FollowerId == userId && fr.IsApproved)
                .Select(fr => fr.Followee)
                .ToListAsync();
        }

        public async Task<bool> IsOwnerOrAdmin(string? OwnerName, ClaimsPrincipal user) 
        {
            var loggedUser = GetCurrentLoggedInUser(user);
            if (loggedUser == null) 
                return false;
            var isInRoleAdmin = await _signInManager.UserManager.IsInRoleAsync(loggedUser, UserRoles.Admin);
            var isInRoleModerator = await _signInManager.UserManager.IsInRoleAsync(loggedUser, UserRoles.Moderator);
           
            if (isInRoleAdmin ||  isInRoleModerator || loggedUser.UserName == OwnerName)
                return true;
            return false;
        }
    }
}