using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace Luxa.Models

{
	public class Photo
	{
		//[Key]
		public int Id { get; set; }
		public UserModel Owner { get; set; } = default!;
		[DisplayName("Podaj nazwę zdjęcia")]
		public string Name { get; set; } = default!;
		[DisplayName("Podaj opis zdjęcia")]
		public string Description { get; set; } = default!;
		public DateTime AddTime { get; set; } = DateTime.Now;

		[NotMapped]
		[DisplayName("Zaladuj zdjecie")]
		public IFormFile ImageFile { get; set; } = default!;
		public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = [];

	}
}
