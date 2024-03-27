namespace Luxa.Models
{
	public class NotificationModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public bool IsViewed { get; set; } = false;
		public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = new List<UserNotificationModel>();


	}
}