using Luxa.Data;
using Luxa.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luxa.Services
{
	public class HomeService : IHomeService
	{
		public List<SelectListItem> GetOrderBySelectListItem()
		{
			var orderByList = typeof(OrderByOptions)
							.GetFields()
							.Select(f => new SelectListItem
							{
								Text = f.GetValue(null)?.ToString() ?? string.Empty,
								Value = f.Name
							})
							.ToList();

			// Dodaj opcję domyślną na początku listy
			orderByList.Insert(0, new SelectListItem { Text = "Nie sortuj", Value = "" });
			return orderByList;
		}
		public List<SelectListItem> GetCategoriesSelectListItem() 
		{
			throw new NotImplementedException();
		}
	}
}
