using Luxa.Models;

namespace Luxa.Interfaces
{
	public interface IPhotoRepository
	{
		Task<Photo?> GetPhotoById(int Id);
		bool Save();
		bool Add(Photo photo);
		Task<IEnumerable<Photo>> GetAllPhotos();
	}
}
