using Luxa.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxa.Models
{
	public class ContactModel
	{
		public int Id { get; set; }
		public UserModel Sender { get; set; } = default!;
		public string? UserName { get; set; }
		[EnumDataType(typeof(CategoryOfContact))]
		public CategoryOfContact Category { get; set; } = default!;
		[Column("CategoryString")]
		public string CategoryAsString
		{
			get { return Category.ToString(); }
			private set { }
		}
		public string DetailedCategory { get; set; } = default!;
		public string Description { get; set; } = default!;

	}
}
