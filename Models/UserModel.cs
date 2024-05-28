using Microsoft.AspNetCore.Identity;

namespace Luxa.Models
{
	public class UserModel : IdentityUser
	{

		public string? Country { get; set; } = default!;
		public string? FirstName { get; set; } = default!;
		public string? LastName { get; set; } = default!;
		//public int Rep { get; set; } = 0;
		//public bool isPrivate { get; set; } = false;
		public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = [];
		public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = [];


	}
}
