using Luxa.Models;

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
    }
}
