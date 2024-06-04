using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Repository
{
	public class PhotoRepository : IPhotoRepository
	{
		private readonly ApplicationDbContext _context;
		public PhotoRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public Task<IEnumerable<Photo>> GetAllPhotos()
		{
			throw new NotImplementedException();
		}

		public IQueryable<Photo> GetPhotosAsync(int pageNumber, int pageSize)
			=> _context.Photo
				.OrderByDescending(photo => photo.AddTime)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);

		public IQueryable<Photo> GetLikedPhotos(UserModel user)
			=> _context.Users
				.Where(u => u.Id == user.Id)
				.SelectMany(u => u.UserLikedPhotos)
				.Select(u => u.Photo);

		public Photo GetPhotoById(int idPhoto)
			=> _context.Photo
				.Where(e => e.Id == idPhoto)
				.First();


		public bool Save()
			=> _context.SaveChanges() > 0;

		public async Task<bool> SaveAsync()
			=> await _context.SaveChangesAsync() > 0;

		public bool Add(Photo photo)
		{
			_context.Add(photo);
			return Save();
		}

		public bool RemoveLikeFromPhoto(UserPhotoModel userPhoto)
		{
			_context.UserLikedPhotos.Remove(userPhoto);
			return Save();
		}
		public bool AddLikeFromPhoto(UserPhotoModel userPhoto)
		{
			_context.UserLikedPhotos.Add(userPhoto);
			return Save();
		}

		public UserPhotoModel? GetUserPhotoModelByPhoto(int idPhoto, UserModel user)
			=> _context.UserLikedPhotos.FirstOrDefault(e => e.PhotoId == idPhoto && e.UserId == user.Id);

		public async Task<Photo?> GetPhotoIncludedPhotoTags(int photoId)
			=> await _context.Photo
				.Include(p => p.PhotoTags)
				.FirstOrDefaultAsync(p => p.Id == photoId);


	}
}
