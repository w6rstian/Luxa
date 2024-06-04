using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Repository
{
	public class ContactRepository : IContactRepository
	{
		private readonly ApplicationDbContext _context;
		public ContactRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public bool Save()
			=> _context.SaveChanges() > 0;

		public async Task<bool> SaveAsync()
			=> await _context.SaveChangesAsync() > 0;

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

		public async Task<IEnumerable<ContactModel>> GetAllContact()
		{
			return await _context.Contacts.ToListAsync();
		}

		public async Task<ContactModel?> GetContactById(int Id)
			=> await _context.Contacts.FindAsync(Id);


	}
}
