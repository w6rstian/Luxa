using Luxa.Models;
using Luxa.ViewModel;

namespace Luxa.Interfaces
{
    public interface IPhotoService
    {
        Task<bool> Create(UserModel user);//get
        Task<bool> Create(Photo photo, UserModel user, string tags);//post
        Task<bool> Edit();//get
        Task<bool> Edit(int id, Photo photo, UserModel user);//post
        Task<bool> Delete();//get
        Task<bool> Delete(int id, Photo photo, UserModel user);//post
                                                               //List<Photo>[] Prototyp(List<Photo> photos, int columnHeight);
        ///LimitedHeightPhotosVM GetAmountOfPhotos(int quantity,int height);
        Task<List<PhotoWithIsLikedVM>> GetPhotosWithIsLikedAsync(int pageNumber, int pageSize, UserModel user);
        Task<List<PhotoWithIsLikedVM>> GetPhotosWithIsLikedForProfileAsync(int pageNumber, int pageSize, UserModel user);
        Task<List<PhotoWithIsLikedVM>> GetPhotosWithIsLikedForDiscoverAsync(int pageNumber, int pageSize, UserModel user, string? tag = "", string? category="", bool order = false, string? sortBy="" );
        Task<List<Photo>> GetLikedPhotos(UserModel user);
        bool IsPhotoLiked(int idPhoto, List<Photo> photos);
		bool LikePhoto(int idPhoto, UserModel user);
        bool UnlikePhoto(int idPhoto, UserModel user);
        bool IncrementViewCountAsync(List<Photo> photo);

	}
}
