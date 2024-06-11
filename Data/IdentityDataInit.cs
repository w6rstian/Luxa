using Luxa.Models;
using Microsoft.AspNetCore.Identity;

namespace Luxa.Data
{
    public static class IdentityDataInit
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Role
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Regular))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Regular));
                if (!await roleManager.RoleExistsAsync(UserRoles.Moderator))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                if (!await roleManager.RoleExistsAsync(UserRoles.Verified))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Verified));

                //Admin
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                string adminUserEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new UserModel()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };
                    /*var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					var notifications = context.Notifications.ToList();*/
                    var notifacationForAdmin = new NotificationModel
                    {
                        Title = "Odpowiedzialność",
                        Description = "Strasznie wczoraj się zasiedziałem. Kodowałem do piątej rano." +
                        " Film mi się urwał jak leżałem na fotelu. Teraz mnie krzyż boli." +
                        " Trochę się przespałem ale musiałem wstać rano bo mam obowiązki." +
                        " Mam wf. Niektórzy mówią że nie można długo siedzieć jak się ma wf na rano ale to nie prawda." +
                        " Można, tylko trzeba wypić energola. Na tym polega odpowiedzialność"
                    };
                    newAdminUser.UserNotifiacations.Add(new UserNotificationModel { User = newAdminUser, Notification = notifacationForAdmin });


                    await userManager.CreateAsync(newAdminUser, "123456789");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
            }
        }


    }
}
