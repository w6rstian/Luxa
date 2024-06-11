using Luxa.Data.Enums;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luxa.Interfaces
{
    public interface IContactService
    {
        //Task<bool> SaveUser(UserModel userModel);
        //UserModel? GetCurrentLoggedInUser(ClaimsPrincipal user);
        CategoryOfContact? GetEnumCategory(string category);
        //string? GetDetailedCategoryName(string detailedCategory);
        //Task<IEnumerable<ContactModel>> GetAllContact();
        List<SelectListItem> GetCategorySelectItems();
        List<SelectListItem> GetDetailedCategorySelectItems();
        List<SelectListItem> GetStateSelectItems(bool isAllIncluded);
        Task<bool> UpdateState(List<KeyValuePair<int, string>> changedStateList);
        Tuple<int, string>? GetTupleFromData(string data);
        //bool Add(ContactModel contactModel);
        //bool Update(ContactModel contactModel);
        //bool Delete(ContactModel contactModel);
        //bool Save();
        //public Task<bool> SaveAsync();
        Task<bool> PrepareToUpdateState(string data);
        void CreateContact(bool isValid, UserModel? user, CategoryOfContact? category, string Description, string DetailedCategory);
        Task<List<ContactListVM>> ShowContacts();
        Tuple<List<string>, List<string>> GetTextAndValueToSelect(string selectedValue);

    }
}
