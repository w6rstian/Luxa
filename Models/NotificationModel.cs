namespace Luxa.Models
{
	public class NotificationModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public ICollection<UserNotificationModel> UserNotifications { get; set; } = [];


	}
}