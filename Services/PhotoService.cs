using Luxa.Data;
using Luxa.Interfaces;
using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


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
            photo.UserId = user;
            //save image into wwwroot
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(photo.ImageFile.FileName);
            string extension = Path.GetExtension(photo.ImageFile.FileName);
            photo.Name = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                photo.ImageFile.CopyToAsync(fileStream);
            }

            _context.Add(photo);
            var result = await _context.SaveChangesAsync();
            return true;


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
    }
}
