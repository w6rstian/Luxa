using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
namespace Luxa.Models

{
    public class Photo
    {
        //[Key]
        public int Id { get; set; }
        public UserModel UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AddTime { get; set; } = DateTime.Now;

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

    }
}
