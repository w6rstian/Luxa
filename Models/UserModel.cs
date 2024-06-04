using Microsoft.AspNetCore.Identity;

namespace Luxa.Models
{
	public class UserModel : IdentityUser
	{

		public string? Country { get; set; } = default!;
		public string? FirstName { get; set; } = default!;
		public string? LastName { get; set; } = default!;

		public int Reputation { get; set; } = 0;

		//{
		//	get
		//	{
		//		return Photos?.Sum(e => e.LikeCount) ?? 0;
		//	}
		//	private set { }
		//}
		public bool isPrivate { get; private set; } = false;
		//public Photo ProfilePhoto { get; set; }
		public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = [];
		public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = [];
		public /*virtual*/ ICollection<Photo> Photos { get; set; } = [];


	}
}
