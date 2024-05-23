using Luxa.Data;
using Luxa.Data.Enums;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
					State = contact.State
				};
				contactsToDisplay.Add(contactListVM);
			}
			//return View(contactsToDisplay);
			ViewBag.CategorySelectItems = _contactService.GetCategorySelectItems();
			ViewBag.DetailedCategorySelectItems = _contactService.GetDetailedCategorySelectItems();
			ViewBag.StateSelectItems = _contactService.GetStateSelectItems(true);
			ViewBag.StateSelectChangeItems = _contactService.GetStateSelectItems(false);
			return View(contactsToDisplay);
		}
		[HttpPost]
		public bool EditState(string data)
		{
			_contactService.PrepareToUpdateState(data);
			return _contactService.Save();

			//var deserializedData = JsonSerializer.Deserialize<>
		}

		/*[HttpPost]
		public async Task<IActionResult> SendChanges()
		{
			try
			{
				Console.WriteLine(await _contactService.SaveAsync());

				return Ok();
			}
			catch (Exception ex)
			{
				// Log the exception
				// return a proper error response
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}*/

		/*[HttpPost]
		public IActionResult AddStateToTempData(string data)
		{
			try
			{
				List<KeyValuePair<int, string>> changedStateList;
				if (TempData.ContainsKey("changedStateList") && null != TempData["changedStateList"])
				{
					changedStateList = JsonSerializer.Deserialize<List<KeyValuePair<int, string>>>(TempData["changedStateList"].ToString());
				}
				else
				{
					changedStateList = new List<KeyValuePair<int, string>>();
				}
				var keyValuePair = _contactService.GetTupleFromData(data);
				if (keyValuePair != null)
				{
					changedStateList.Add((KeyValuePair<int, string>)keyValuePair);
				}


				TempData["changedStateList"] = JsonSerializer.Serialize(changedStateList);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
		}

		public void SendTempDataToDatabase()
		{
			List<KeyValuePair<int, string>> changedStateList;

			// Odczytywanie listy z TempData
			if (!TempData.ContainsKey("changedStateList"))
			{
				ViewData["Message"] = "Zmiana stanu nie nastąpiła, nie ma czego zmienić";
				return;
			}
			changedStateList = TempData["changedStateList"] as List<KeyValuePair<int, string>>;
			if (!_contactService.UpdateState(changedStateList).Result)
			{
				ViewData["Message"] = "Zmiana stanu nie powiodło się";
				return;
			}
			ViewData["Message"] = "Zmiana powiodła się";


		}*/




		/*
		public void AddCookieToList(string idChangedState, string stateName)
		{
			List<HttpCookie> cookieList;
			if (Request.Cookies["CookieList"] != null)
			{
				string json = Request.Cookies["CookieList"].Value;
				cookieList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HttpCookie>>(json);
			}
			else
			{
				cookieList = new List<HttpCookie>();
			}

			HttpCookie newCookie = new HttpCookie(idChangedState)
			{
				Value = stateName,
				Expires = DateTime.Now.AddDays(1)
			};
			cookieList.Add(newCookie);

			string updatedJson = Newtonsoft.Json.JsonConvert.SerializeObject(cookieList);
			HttpCookie listCookie = new HttpCookie("CookieList", updatedJson)
			{
				Expires = DateTime.Now.AddDays(1)
			};
			Response.Cookies.Add(listCookie);
		}


		public ActionResult GetCookiesFromList()
		{
			List<HttpCookie> cookieList;
			if (Request.Cookies["CookieList"] != null)
			{
				string json = Request.Cookies["CookieList"].Value;
				cookieList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HttpCookie>>(json);
			}
			else
			{
				cookieList = new List<HttpCookie>();
			}

			ViewBag.CookieList = cookieList;
			return View();
		}
		*/




		//[HttpPost]
		//public IActionResult SubmitChanges([FromBody] Dictionary<string, string> changes)
		//{
		//	foreach (var change in changes)
		//	{
		//		var recordId = change.Key;
		//		var newValue = change.Value;

		//		// Twoja logika do aktualizacji rekordu w bazie danych
		//		UpdateRecord(recordId, newValue);
		//	}

		//	return Json(new { success = true });
		//}

		//private void UpdateRecord(string recordId, string newValue)
		//{
		//	// Implementacja aktualizacji rekordu w bazie danych
		//}

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