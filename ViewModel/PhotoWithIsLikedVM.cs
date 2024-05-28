using Luxa.Models;

namespace Luxa.ViewModel
{
	public class PhotoWithIsLikedVM
	{
		public Photo Photo { get; set; } = default!;
		public bool IsLiked { get; set; } = false;
	}
}
