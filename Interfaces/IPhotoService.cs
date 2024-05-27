using Luxa.Models;
using Luxa.ViewModel;

namespace Luxa.Interfaces
{
    public interface IPhotoService
    {
        Task<bool> Create(UserModel user);//get
        Task<bool> Create(Photo photo, UserModel user);//post
        Task<bool> Edit();//get
        Task<bool> Edit(int id, Photo photo, UserModel user);//post
        Task<bool> Delete();//get
        Task<bool> Delete(int id, Photo photo, UserModel user);//post
        //List<Photo>[] Prototyp(List<Photo> photos, int columnHeight);
		///LimitedHeightPhotosVM GetAmountOfPhotos(int quantity,int height);
        Task<List<Photo>> GetPhotosAsync(int pageNumber, int pageSize);

	}
}
