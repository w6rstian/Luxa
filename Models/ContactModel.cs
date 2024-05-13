using Luxa.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Luxa.Models
{
	public class ContactModel
	{
		public int Id { get; set; }
		public UserModel Sender { get; set; } = default!;
		[EnumDataType(typeof(CategoryOfContact))]
		public CategoryOfContact Category { get; set; } = default!;
		public string DetailedCategory { get; set; } = default!;
		public string Description { get; set; } = default!;

	}
}
