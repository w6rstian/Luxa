using Luxa.Models;

namespace Luxa.ViewModel
{
	public class UserNotificationsVM
	{
		public IQueryable<NotificationVM> Notifications { get; set; } = default!;
		public UserModel User { get; set; } = default!;
	}
}
