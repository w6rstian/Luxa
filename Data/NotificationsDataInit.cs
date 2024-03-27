using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Data
{
	public static class NotificationsDataInit
	{
		public static void SeedNotifications(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

				if (context.Notifications.Any())
				{
					return;
				}
				context.Notifications.AddRange(
					new NotificationModel
					{
						Title = "Pełne Dane użytkownika",
						Description = "Aby mieć możliwość korzystania w pełni z naszej aplikacji, gorąco zachęcamy do uzupełniania danych konta aby cieszyć się z Luxy w pełni!"
					},
					new NotificationModel
					{
						Title = "Intensywny rozwój Luxa",
						Description = "Trwają intensywne prace nad stroną. Zarząd IT Luxa prosi o cierpliwość i wyrozumiałość z powodu braku części funkcjonalności. Dzień po dniu będziemy je wdrażać w trosce o naszą społeczność."
					}
				);
				context.SaveChanges();
			}
		}
	}
}
