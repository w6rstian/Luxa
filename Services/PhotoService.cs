using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Luxa.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly ApplicationDbContext _context; //wstrzykiwanie kontekstu
		private readonly IPhotoRepository _photoRepository;
		private readonly UserManager<UserModel> _userManager;
		private readonly IWebHostEnvironment _hostEnvironment;

		public PhotoService(ApplicationDbContext context, UserManager<UserModel> userManager, IWebHostEnvironment hostEnvironment, IPhotoRepository photoRepository)
		{
			_context = context;
			_userManager = userManager;
			_hostEnvironment = hostEnvironment;
			_photoRepository = photoRepository;
		}

		public async Task<bool> Create(UserModel user)
		{
			return true;
		}

		public async Task<bool> Create(Photo photo, UserModel user)
		{
			photo.Owner = user;
			//save image into wwwroot
			string wwwRootPath = _hostEnvironment.WebRootPath;
			string fileName = Path.GetFileNameWithoutExtension(photo.ImageFile.FileName);
			string extension = Path.GetExtension(photo.ImageFile.FileName);
			photo.Name = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			string path = Path.Combine(wwwRootPath + "/Image", fileName);
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await photo.ImageFile.CopyToAsync(fileStream);
			}

			//user.Photos.Add(photo);
			return _photoRepository.Add(photo);


		}


		public Task<bool> Delete()
		{
			throw new NotImplementedException();
		}

		public Task<bool> Delete(int id, Photo photo, UserModel user)
		{
			throw new NotImplementedException();
		}

		public async Task Edit(int id, Photo photo, UserModel user)
		{

			_context.Update(photo);

		}

		public Task Edit(Photo photo, UserModel user)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Edit()
		{
			throw new NotImplementedException();
		}

		Task<bool> IPhotoService.Edit(int id, Photo photo, UserModel user)
		{
			throw new NotImplementedException();
		}


		//      public List<Photo>[] Prototyp(List<Photo> photos,int columnHeight)
		//      { 
		//	photos.OrderBy(photo => photo.Height);
		//	List<Photo>[] arrayOfLists = new List<Photo>[3];
		//          int totalHeight = 0;
		//          foreach (var item in arrayOfLists)
		//          {
		//              foreach (Photo photo in photos)
		//              {
		//                  if (totalHeight + photo.Height <= columnHeight)
		//                  {
		//                      item.Add(photo);
		//                      photos.Remove(photo);
		//                      totalHeight += photo.Height;
		//                  }
		//              }
		//          }
		//          return arrayOfLists;
		//}

		//public LimitedHeightPhotosVM GetAmountOfPhotos(int quantity, int height)
		//{
		//          List<Photo> myPhotos = _context.Photo
		//	.Where(photo => photo.Height < height)
		//	.OrderByDescending(photo => photo.AddTime)
		//	.Take(quantity)
		//	.ToList();

		//          if (myPhotos.Count < quantity) 
		//          {


		//          }
		//	return new LimitedHeightPhotosVM
		//	{
		//		photos = myPhotos,
		//		isFoundedRightQuantity = null
		//	};

		//}
		public bool IsPhotoLiked(int idPhoto, List<Photo> photos)
			=> photos.Select(e => e.Id).Contains(idPhoto);

		public bool LikePhoto(int idPhoto, UserModel user)
		{
			var photo = _photoRepository.GetPhotoById(idPhoto);
			var userPhoto = new UserPhotoModel
			{
				PhotoId = idPhoto,
				Photo = photo,
				User = user,
				UserId = user.Id
			};
			return _photoRepository.AddLikeFromPhoto(userPhoto);
		}
		public bool UnlikePhoto(int idPhoto, UserModel user)
		{
			var userPhoto = _photoRepository.GetUserPhotoModelByPhoto(idPhoto, user);
			return (userPhoto != null) && _photoRepository.RemoveLikeFromPhoto(userPhoto);
		}

		public async Task<List<PhotoWithIsLikedVM>> GetPhotosWithIsLikedAsync(int pageNumber, int pageSize, UserModel user)
		{
			var allPhotos = await _photoRepository.GetPhotosAsync(pageNumber, pageSize)
				.Include(photo => photo.Owner)
				.ToListAsync();
			var likedPhotos = await GetLikedPhotos(user);
			var likedPhotoIds = new HashSet<int>(likedPhotos.Select(p => p.Id));
			var photosWithIsLiked = allPhotos.Select(photo => new PhotoWithIsLikedVM
			{
				Photo = photo,
				IsLiked = likedPhotoIds.Contains(photo.Id),
				OwnerName = photo.Owner.UserName
			}).ToList();
			return photosWithIsLiked;


		}

		public async Task<List<Photo>> GetLikedPhotos(UserModel user)
			=> await _photoRepository.GetLikedPhotos(user).ToListAsync();
	}
}
/*int totalHeight = 0;

        // Sortowanie zdjęć według wysokości malejąco
        var sortedPhotos = photos.OrderByDescending(p => p.Height).ToList();

        foreach (var photo in sortedPhotos)
        {
            if (totalHeight + photo.Height <= columnHeight)
            {
                selectedPhotos.Add(photo);
                totalHeight += photo.Height;
            }
        }
*/