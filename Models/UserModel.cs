using Microsoft.AspNetCore.Identity;

namespace Luxa.Models
{
	public class UserModel : IdentityUser
	{

		public string? Country { get; set; } = default!;
		public string? FirstName { get; set; } = default!;
		public string? LastName { get; set; } = default!;
		//public int Reputation 
		//{ 
		//	get 
		//	{
  //              return Photos.Sum(e => e.LikeCount);
  //          } 
		//	private set { } 
		//}
		public bool isPrivate { get; private set; } = false;
		public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = [];
		public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = [];
		//public ICollection<Photo> Photos { get; set; } = [];


    }
}
