using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Luxa.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing;


namespace Luxa.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext _context; //wstrzykiwanie kontekstu

        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PhotoService(ApplicationDbContext context, UserManager<UserModel> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;

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

            _context.Add(photo);

            return await _context.SaveChangesAsync() >0 ? true : false;


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
		public bool IsPhotoLiked(int idPhoto,List<Photo> photos)
		    => photos.Select(e => e.Id).Contains(idPhoto);

		public async Task<List<Photo>> GetPhotosAsync(int pageNumber, int pageSize)
			=> await _context.Photo
				.OrderByDescending(photo => photo.AddTime)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
        public async Task<List<Photo>> GetLikedPhotos(UserModel user) 
            => await _context.Users
                .Where(u => u.Id == user.Id)
                .SelectMany(u => u.UserLikedPhotos)
                .Select(u => u.Photo)
				.ToListAsync();
		public bool LikePhoto(int idPhoto, UserModel user)
		{
            var photo = GetPhotoById(idPhoto);
			var userPhoto = new UserPhotoModel
            {
                PhotoId = idPhoto,
                Photo = photo,
                User = user,
                UserId = user.Id
            };
            return AddLikePhoto(userPhoto);
		}
        public Photo GetPhotoById(int idPhoto) 
            => _context.Photo
            .Where(e => e.Id==idPhoto)
            .First();

        public UserPhotoModel? GetUserPhotoModelByPhoto(int idPhoto, UserModel user) 
            => _context.UserLikedPhotos.FirstOrDefault(e => e.PhotoId == idPhoto && e.UserId == user.Id);

		public bool UnlikePhoto(int idPhoto,UserModel user)
		{
            var userPhoto = GetUserPhotoModelByPhoto(idPhoto, user);
            return (userPhoto !=null) ? RemoveLikePhoto(userPhoto) : false;
		}
        public bool RemoveLikePhoto(UserPhotoModel userPhoto) 
        {
			_context.UserLikedPhotos.Remove(userPhoto);
			return _context.SaveChanges() > 0;
		}
        public bool AddLikePhoto(UserPhotoModel userPhoto) 
        {
            _context.UserLikedPhotos.Add(userPhoto);
            return _context.SaveChanges() > 0;

		}

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