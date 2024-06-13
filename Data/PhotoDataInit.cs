using Luxa.Models;
using Microsoft.AspNetCore.Identity;

namespace Luxa.Data
{
	public static class PhotoDataInit
	{
		public static async Task SeedPhotos(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
				var admin = await userManager.FindByNameAsync("admin");
				if (admin != null)
				{
					TagModel tag = new TagModel()
					{
						TagName = "góra"
					};

					//Photo photo = new Photo()
					//{
					//	Owner = admin,
					//	Name = "krajobraz.jpg",
					//	Description = "Krajobraz"
					//};
					//if (!AddTagsToPhoto(photo, List<>))
				}
			} }
	}
}

