using Microsoft.AspNetCore.Identity;

namespace Luxa.Models
{
	public class UserModel : IdentityUser
	{

		public string? Country { get; set; } = default!;

		public string? FirstName { get; set; } = default!;

		public string? LastName { get; set; } = default!;

	}
}
