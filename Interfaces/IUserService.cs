using Luxa.Models;
using System.Security.Claims;

namespace Luxa.Interfaces
{
	public interface IUserService
	{
		UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user);
		Task<bool> SaveUser(UserModel userModel);
	}
}
