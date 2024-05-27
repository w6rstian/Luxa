using Luxa.Interfaces;
using Luxa.Models;

namespace Luxa.Repository
{
	public class PhotoRepository : IPhotoRepository
	{
		public bool Add(Photo photo)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Photo>> GetAllPhotos()
		{
			throw new NotImplementedException();
		}

		public Task<Photo?> GetPhotoById(int Id)
		{
			throw new NotImplementedException();
		}

		public bool Save()
		{
			throw new NotImplementedException();
		}
	}
}
