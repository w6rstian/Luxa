using Luxa.Data;
using Luxa.Data.Enums;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Security.Claims;

namespace Luxa.Services
{
	public class ContactService : IContactService
	{



		private readonly SignInManager<UserModel> _signInManager;
		private readonly UserManager<UserModel> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly NotificationService _notificationService;

		public ContactService(ApplicationDbContext context, SignInManager<UserModel> signInManager,
			UserManager<UserModel> userManager, NotificationService notificationService)
		{
			_context = context;
			_signInManager = signInManager;
			_userManager = userManager;
			_notificationService = notificationService;
		}
		public bool Save()
		{ 
			var saved = _context.SaveChanges();
			return saved > 0;
		}
		public bool Add(ContactModel contactModel) 
		{
			//add nie dodaje zmian, save jest za to odpowiedzialny
			_context.Add(contactModel);
			return Save();
		}
		public bool Delete(ContactModel contactModel) 
		{
			_context.Remove(contactModel);
			return Save();
		}
		public UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user)
		{
			var currentUserTask = _userManager.GetUserAsync(user);
			currentUserTask.Wait();
			return currentUserTask.Result;
		}
		public async Task<bool> SaveUser(UserModel userModel)
		{
			var result = await _userManager.UpdateAsync(userModel);
			return result.Succeeded;
		}

		public CategoryOfContact? GetEnumCategory(string category)
		{
			foreach (CategoryOfContact item in Enum.GetValues(typeof(CategoryOfContact)))
			{
				if (category.Equals(item.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					return item;
				}
			}
			return null;
		}

		//public string? GetDetailedCategoryName(string detailedCategory)
		//{
		//	FieldInfo[] fields = typeof(DatailedContactCategories).GetFields(BindingFlags.Public | BindingFlags.Static);
		//	foreach (var field in fields)
		//	{
		//		if ((string)field.GetValue(null) == detailedCategory)
		//		{
		//			return field.Name;
		//		}
		//	}
		//	return null;
		//}
	}
}
