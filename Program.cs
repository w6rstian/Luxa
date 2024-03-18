using Luxa.Data;
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
			//£¹czenie siê z baz¹ danych
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				//appsettings.json zawiera w sobie connection string skopiowany z w³aœciwoœci z baz danych
				options.UseSqlServer(builder.Configuration.GetConnectionString("LuxaDb"));
			});


			var app = builder.Build();

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
