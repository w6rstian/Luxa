namespace Luxa.Models
{
	public class UserPhotoModel
	{
		public string UserId { get; set; }
		public UserModel User { get; set; }

		public int PhotoId { get; set; }
		public Photo Photo { get; set; }
	}
}
