using Luxa.Data;
using Luxa.Models;
using Luxa.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luxa
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			//��czenie si� z baz� danych
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				//appsettings.json zawiera w sobie connection string skopiowany z w�a�ciwo�ci z baz danych a "LuxaDb" to odwo�anie do appsettings.json
				options.UseSqlServer(builder.Configuration.GetConnectionString("LuxaDb"));
			});
			builder.Services.AddIdentity<UserModel,IdentityRole>(options => 
			{
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = false;
			}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/SignIn";						 
			});
			builder.Services.AddScoped<NotificationService>();
			var app = builder.Build();

			NotificationsDataInit.SeedNotifications(app);
			IdentityDataInit.SeedUsersAndRolesAsync(app);











			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			//Routing do logowania
			app.MapControllerRoute(
				name: "SignIn",
				pattern: "signin",
				defaults: new { controller = "Account", action = "SignIn" });
			//Routing do Rejestracji
			app.MapControllerRoute(
				name: "SignUp",
				pattern: "signup",
				defaults: new { controller = "Account", action = "SignUp" });

			app.Run();
		}
	}
}
