using Luxa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxa.ViewModel
{
	public class ContactListVM
	{
		public int Id { get; set; }
		public string? UserName { get; set; }
		[EnumDataType(typeof(CategoryOfContact))]
		public CategoryOfContact Category { get; set; } = default!;
		public string DetailedCategory { get; set; } = default!;
		public string Description { get; set; } = default!;
		[EnumDataType(typeof(ContactState))]
		public ContactState State { get; set; } = default!;

	}
}
