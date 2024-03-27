using Luxa.Models;

namespace Luxa.ViewModel
{
	public class UsersListVM
	{
		public IQueryable<NotificationModel> Notifications { get; set; }
		public UserModel User { get; set; }
		public IList<string> Roles { get; set; }
	}
}
