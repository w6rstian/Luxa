using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface IContactRepository
    {
        Task<ContactModel?> GetContactById(int Id);
        bool Save();
        bool Add(ContactModel contactModel);
        Task<IEnumerable<ContactModel>> GetAllContact();
    }
}
