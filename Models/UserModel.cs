using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Models
{
	public class UserModel : IdentityUser
	{
		
		public string? Country { get; set; } = default!;

		public string? FirstName { get; set; } = default!;

		public string? LastName { get; set; } = default!;
		public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = new List<UserNotificationModel>();


	}
}
