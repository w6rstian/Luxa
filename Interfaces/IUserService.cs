using Luxa.Models;
using System.Security.Claims;

namespace Luxa.Interfaces
{
    public interface IUserService
    {
        UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user);
        Task<bool> SaveUser(UserModel userModel);
        Task<bool> UpdateReputation(UserModel userModel);
        UserModel? GetCurrentLoggedInUserWithPhotos(ClaimsPrincipal user);
        Task<bool> IsUserWithUserNameExist(string userName);
        Task<UserModel?> GetUserByUserName(string userName);
        Task<bool> RemoveUserById(string Id);
        Task<bool> IsFollowing(string followerId, string followeeId);
        Task<List<FollowModel>> GetPendingFollowRequests(string userId);
        Task<List<UserModel>> GetFollowedUsers(string userId);
        Task<bool> IsOwnerOrAdmin(string? OwnerName, ClaimsPrincipal user);
    }
}
