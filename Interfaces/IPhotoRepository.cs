using Luxa.Models;

namespace Luxa.Interfaces
{
    public interface IPhotoRepository
    {
        Photo GetPhotoById(int idPhoto);
        Task<Photo> GetPhotoByIdAsync(int idPhoto);

        bool Save();
        bool Add(Photo photo);
        Task<IEnumerable<Photo>> GetAllPhotos();
        UserPhotoModel? GetUserPhotoModelByPhoto(int idPhoto, UserModel user);
        bool AddLikeFromPhoto(UserPhotoModel userPhoto);
        bool RemoveLikeFromPhoto(UserPhotoModel userPhoto);
        IQueryable<Photo> GetLikedPhotos(UserModel user);
        IQueryable<Photo> GetPhotosAsync();
        IQueryable<Photo> GetPhotosAsync(int pageNumber, int pageSize);
        IQueryable<Photo> GetPhotosOwnByUserAsync(int pageNumber, int pageSize, UserModel user);
        Task<Photo?> GetPhotoIncludedPhotoTags(int idPhoto);
        bool LikeCount(Photo photo);
    }
}
