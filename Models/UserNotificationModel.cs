namespace Luxa.Models
{
	public class UserNotificationModel
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public UserModel User { get; set; }

		public int NotificationId { get; set; }
		public NotificationModel Notification { get; set; }
	}
}
