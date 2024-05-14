using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Luxa.Data;
using Luxa.Data.Enums;
using System.Reflection;
using Luxa.ViewModel;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;

namespace Luxa.Controllers
{
	public class ContactController : Controller
	{
		private readonly IContactService _contactService;
		public ContactController(IContactService contactService)
		{
			_contactService = contactService;
		}
		public IActionResult UserContact()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UserContact(ContactVM contactVM)
		{
			var user = _contactService.GetCurrentLoggedInUser(User);
			var category = _contactService.GetEnumCategory(contactVM.Category);

			if (ModelState.IsValid && user != null && category != null)
			{

				var contactModel = new ContactModel
				{
					Sender = user,
					UserName = user.UserName,
					Category = (CategoryOfContact)category,
					DetailedCategory = contactVM.DetailedCategory,
					Description = contactVM.Description
				};
				_contactService.Add(contactModel);
			}
			return View();
		}
		[HttpGet]
		public IActionResult GetDetailedCategory(string selectedValue)
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
			//var detailedCategories = new List<SelectListItem>();
			//detailedCategories.AddRange(filteredList.Select(p => new SelectListItem { Text = p.Item2, Value = p.Item1 }));
			return Json(new { text = filteredText, value = filteredValue });
		}
		public async Task<IActionResult> ContactList()
		{
			var contacts = await _contactService.GetAllContact();
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
				};
				contactsToDisplay.Add(contactListVM);
			}
			//return View(contactsToDisplay);
			ViewBag.CategorySelectItems = _contactService.GetCategorySelectItems();
			ViewBag.DetailedCategorySelectItems = _contactService.GetDetailedCategorySelectItems();
			ViewBag.StateSelectItems = _contactService.GetStateSelectItems();
			return View(contactsToDisplay);
		}
		/*
		 		public async Task<IActionResult> UsersList()
		{
			var users = _context.Users.ToList();
			var usersWithRoles = new List<UsersListVM>();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				var notifications = _notificationService.GetNotificationsForUser(user.Id);

				var userListVM = new UsersListVM
				{
					User = user,
					Roles = roles,
					Notifications = notifications
				};
				usersWithRoles.Add(userListVM);
			}

			return View(usersWithRoles);
		}
		 */




	}
}