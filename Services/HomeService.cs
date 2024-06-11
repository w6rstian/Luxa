using Luxa.Data;
using Luxa.Data.Enums;
using Luxa.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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
            //orderByList.Insert(0, new SelectListItem { Text = "Nie sortuj", Value = "" });
            return orderByList;
        }
        public List<SelectListItem> GetCategoriesSelectListItem()
        {
            var categoryList = Enum.GetValues(typeof(CategoryOfPhotos))
                       .Cast<CategoryOfPhotos>()
                       .Select(e =>
                       {
                           var displayAttribute = typeof(CategoryOfPhotos).GetField(e.ToString())
                               ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                               .SingleOrDefault() as DisplayAttribute;
                           var displayName = displayAttribute?.GetName() ?? e.ToString();
                           return new SelectListItem { Text = displayName, Value = e.ToString() };
                       })
                       .ToList();
            categoryList.Insert(0, new SelectListItem { Text = "Wszystkie", Value = "" });
            return categoryList;

        }
    }
}
