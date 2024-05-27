using Luxa.Data;
using Luxa.Data.Enums;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Luxa.Services
{
	public class ContactService : IContactService
	{
		private readonly ApplicationDbContext _context;
		private readonly NotificationService _notificationService;
		private readonly IContactRepository _contactRepository;

		public ContactService(ApplicationDbContext context, SignInManager<UserModel> signInManager,
			UserManager<UserModel> userManager, NotificationService notificationService, IContactRepository contactRepository)
		{
			_context = context;
			_notificationService = notificationService;
			_contactRepository = contactRepository;
		}
		//sprawdzona
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
		//sprawdzona
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
		//sprawdzona
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
		//sprawdzona
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
				var contactModel = await _contactRepository.GetContactById(item.Key);
				contactModel.State = (ContactState)Enum.Parse(typeof(ContactState), item.Value);
			}
			return _contactRepository.Save();
		}

		//sprawdzona
		public Tuple<int, string>? GetTupleFromData(string data)
		{
			var splited = data.Split(".");
			if (int.TryParse(splited[0], out int id))
				return Tuple.Create(id, splited[1]);
			else
				return null;
		}

		public async Task<bool> PrepareToUpdateState(string data)
		{
			var splitedData = GetTupleFromData(data);
			if (splitedData == null)
			{
				return false;
			}

			var contact = await _contactRepository.GetContactById(splitedData.Item1);
			if (contact == null)
			{
				return false;
			}
			ContactState contactState = (ContactState)Enum.Parse(typeof(ContactState), splitedData.Item2);
			if (contact.State != contactState)
			{
				contact.State = contactState;
			}
			return _contactRepository.Save();
		}
		public void CreateContact(bool isValid, UserModel? user, CategoryOfContact? category, string Description, string DetailedCategory)
		{
			if (isValid && user != null && category != null)
			{

				var contactModel = new ContactModel
				{
					Sender = user,
					UserName = user.UserName,
					Category = (CategoryOfContact)category,
					DetailedCategory = DetailedCategory,
					Description = Description
				};
				_contactRepository.Add(contactModel);

			}
		}
		public async Task<List<ContactListVM>> ShowContacts() 
		{
			var contacts = await _contactRepository.GetAllContact();
			var contactsToDisplay = new List<ContactListVM>();
			foreach (var contact in contacts)
			{
				var contactListVM = new ContactListVM
				{
					Id = contact.Id,
					UserName = contact.UserName,
					Category = contact.Category,
					Description = contact.Description,
					DetailedCategory = contact.DetailedCategory,
					State = contact.State
				};
				contactsToDisplay.Add(contactListVM);
			}
			return contactsToDisplay;
		}
		public Tuple<List<string>, List<string>> GetTextAndValueToSelect(string selectedValue) 
		{
			List<string> filteredText = new();
			List<string> filteredValue = new();
			if (selectedValue == "All")
			{
				filteredText = typeof(DatailedContactCategories)
											   .GetFields(BindingFlags.Public | BindingFlags.Static)
											   .Select(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item2)
											   .ToList();
				filteredValue = typeof(DatailedContactCategories)
											   .GetFields(BindingFlags.Public | BindingFlags.Static)
											   .Select(field => field.Name)
											   .ToList();
			}
			else
			{
				CategoryOfContact category = (CategoryOfContact)Enum.Parse(typeof(CategoryOfContact), selectedValue);
				filteredText = typeof(DatailedContactCategories)
											   .GetFields(BindingFlags.Public | BindingFlags.Static)
											   .Where(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item1 == category)
											   .Select(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item2)
											   .ToList();
				filteredValue = typeof(DatailedContactCategories)
											   .GetFields(BindingFlags.Public | BindingFlags.Static)
											   .Where(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item1 == category)
											   .Select(field => field.Name)
											   .ToList();
			}
			return new Tuple<List<string>, List<string>>(filteredText,filteredValue); 
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
