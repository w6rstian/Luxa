using Luxa.Data;
using Luxa.Data.Enums;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
			=> _context.SaveChanges() > 0;


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

		public async Task<IEnumerable<ContactModel>> GetAllContact()
		{
			return await _context.Contacts.ToListAsync();
		}

		public List<SelectListItem> GetCategorySelectItems()
		{
			List<SelectListItem> categoriesList = new()
			{
				new SelectListItem {Value = "All", Text="Wszystkie"}
			};
			foreach (CategoryOfContact item in Enum.GetValues(typeof(CategoryOfContact)))
			{
				categoriesList.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
			}
			return categoriesList;
		}
		public List<SelectListItem> GetDetailedCategorySelectItems()
		{
			List<string> textList = typeof(DatailedContactCategories)
										   .GetFields(BindingFlags.Public | BindingFlags.Static)
										   .Select(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item2)
										   .ToList();
			List<string> valueList = typeof(DatailedContactCategories)
										   .GetFields(BindingFlags.Public | BindingFlags.Static)
										   .Select(field => field.Name)
										   .ToList();
			var detailedCategoriesList = new List<SelectListItem>
			{
				new() { Value = "All", Text = "Wszystkie" }
			};
			for (int i = 0; i < textList.Count; i++)
				detailedCategoriesList.Add(new SelectListItem { Value = valueList[i], Text = textList[i] });
			return detailedCategoriesList;



		}

		public List<SelectListItem> GetStateSelectItems(bool isAllIncluded)
		{

			var stateList = new List<SelectListItem>();
			if (isAllIncluded)
				stateList.Add(new SelectListItem("Wszystkie", "All"));


			foreach (ContactState item in Enum.GetValues(typeof(ContactState)))
			{
				var selectListItem = new SelectListItem();
				selectListItem.Value = item.ToString();
				selectListItem.Text = item.ToString();
				stateList.Add(selectListItem);
			}
			return stateList;
		}

		public async Task<bool> UpdateState(List<KeyValuePair<int, string>> changedStateList)
		{
			foreach (var item in changedStateList)
			{
				var contactModel = await _context.Contacts.FindAsync(item.Key);
				contactModel.State = (ContactState)Enum.Parse(typeof(ContactState), item.Value);
			}
			return Save();
		}

		public Tuple<int, string>? GetTupleFromData(string data)
		{
			var splited = data.Split(".");
			if (int.TryParse(splited[0], out int id))
				return Tuple.Create(id, splited[1]);
			else
				return null;
		}

		public void PrepareToUpdateState(string data)
		{
			var splitedData = GetTupleFromData(data);
			if (splitedData == null)
			{
				return;
			}

			var contact = _context.Contacts.Find(splitedData.Item1);
			if (contact == null)
			{
				return;
			}
			ContactState contactState = (ContactState)Enum.Parse(typeof(ContactState), splitedData.Item2);
			if (contact.State != contactState)
			{
				contact.State = contactState;
			}
		}

		public async Task<bool> SaveAsync()
			=> await _context.SaveChangesAsync() > 0;







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
