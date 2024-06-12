using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxa.Models
{
    public class UserModel : IdentityUser
    {

        public string? Country { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public int Reputation { get; set; } = 0;
        [NotMapped]
        public string Level
        {
            get
            {
                int[] thresholds = [20, 50, 100, 200, 500, 1000, 5000];
                for (int i = 1; i <= thresholds.Length; i++)
                {
                    if (Reputation < thresholds[i])
                    {
                        return "Lv " + i;
                    }
                }

                return "Lv " + thresholds.Length + 1;
            }
        }
        public bool IsPrivate { get; set; } = false;
        public string? AvatarUrl { get; set; } = "/assets/blank-profile-picture.png";
		public string? BackgroundUrl { get; set; } = "/assets/prostokat.png";
        public string? Description { get; set; } = "Hej, jestem użytkownikiem Luxy";
        public ICollection<UserNotificationModel> UserNotifiacations { get; set; } = [];
        public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = [];
        public ICollection<Photo> Photos { get; set; } = [];


    }
}
