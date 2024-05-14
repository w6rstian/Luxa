using Luxa.Models;

namespace Luxa.ViewModel
{
	public class UsersListVM
	{
		public IQueryable<NotificationVM> Notifications { get; set; } = default!;
		public UserModel User { get; set; } = default!;
		public IList<string> Roles { get; set; } = default!;
	}
}
