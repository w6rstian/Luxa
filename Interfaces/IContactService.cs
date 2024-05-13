using Luxa.Data.Enums;
using Luxa.Models;
using System.Security.Claims;

namespace Luxa.Interfaces
{
	public interface IContactService
	{
		Task<bool> SaveUser(UserModel userModel);
		UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user);
		CategoryOfContact? GetEnumCategory(string category);
		//string? GetDetailedCategoryName(string detailedCategory);





		bool Add(ContactModel contactModel);
		//bool Update(ContactModel contactModel);
		bool Delete(ContactModel contactModel);
		bool Save();

	}
}
