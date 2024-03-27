using System.Threading.Tasks;
using Luxa.Data;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Services
{
	public class NotificationService
	{
		private readonly UserManager<UserModel> _userManager;
		private readonly ApplicationDbContext _context;
		public NotificationService(UserManager<UserModel> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public IQueryable<NotificationModel> GetNotificationsForUser(string userId)
			=> _context.Users
				.Where(u => u.Id == userId)
				.SelectMany(u => u.UserNotifiacations)
				.Select(un => un.Notification);
		public async Task<int> GetNotificationsCountAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
				return 0;
			var notifications = GetNotificationsForUser(user.Id);
			return notifications.Count(n => !n.IsViewed);
		}
	}
}