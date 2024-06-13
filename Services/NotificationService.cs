using Luxa.Data;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
        public IQueryable<NotificationVM> GetNotificationsForUser(string userId)
            => _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserNotifiacations)
                .Select(un => new NotificationVM
                {
                    Id = un.Notification.Id,
                    Title = un.Notification.Title,
                    Description = un.Notification.Description,
                    IsViewed = un.IsViewed
                });

        public async Task<int> GetNotificationsCountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return 0;
            var notifications = GetNotificationsForUser(user.Id);
            return notifications.Count(n => !n.IsViewed);
        }

        public async Task<int> GetNotificationsCountAsync(ClaimsPrincipal user)
        {
			var currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null) 
                return -1;
            return await GetNotificationsCountAsync(currentUser.Id);
		}

        public async Task SendFollowRequestNotification(UserModel followee, UserModel follower)
        {
            var notification = new UserNotificationModel
            {
                UserId = followee.Id,
                Notification = new NotificationModel
                {
                    Title = "Nowa prośba o obserwację",
                    Description = $"{follower.UserName} chce cię obserwować. Przejdź do ustawień konta, aby go zaakceptować."
                }
            };

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendFollowApprovedNotification(UserModel follower, UserModel followee)
        {
            var notification = new UserNotificationModel
            {
                UserId = follower.Id,
                Notification = new NotificationModel
                {
                    Title = "Zatwierdzona prośba o obserwację",
                    Description = $"{followee.UserName} zatwierdził twoja prośbę o obserwowanie."
                }
            };

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SendFollowRejectedNotification(UserModel follower, UserModel followee)
        {
            var notification = new UserNotificationModel
            {
                UserId = follower.Id,
                Notification = new NotificationModel
                {
                    Title = "Odrzucona prośba o obserwację",
                    Description = $"{followee.UserName} odrzucił twoją prośbę o obserwowanie."
                }
            };

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}