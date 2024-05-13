using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Luxa.Data;
using Luxa.Data.Enums;
using System.Reflection;
using Luxa.ViewModel;
using Luxa.Interfaces;
using Luxa.Models;

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

			if (ModelState.IsValid && user!=null && category!=null)
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
			CategoryOfContact category = (CategoryOfContact)Enum.Parse(typeof(CategoryOfContact), selectedValue);
			List<string> filteredText = typeof(DatailedContactCategories)
										   .GetFields(BindingFlags.Public | BindingFlags.Static)
										   .Where(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item1 == category)
										   .Select(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item2)
										   .ToList();
			List<string> filteredValue = typeof(DatailedContactCategories)
										   .GetFields(BindingFlags.Public | BindingFlags.Static)
										   .Where(field => ((ValueTuple<CategoryOfContact, string>)field.GetValue(null)).Item1 == category)
										   .Select(field => field.Name)
										   .ToList();

			var detailedCategories = new List<SelectListItem>();
			//detailedCategories.AddRange(filteredList.Select(p => new SelectListItem { Text = p.Item2, Value = p.Item1 }));
			return Json(new { text = filteredText, value = filteredValue });
		}


	}
}