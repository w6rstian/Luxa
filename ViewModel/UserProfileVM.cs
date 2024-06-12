namespace Luxa.ViewModel
{
    using Luxa.Models;
    using System.Collections.Generic;

    public class UserProfileVM
    {
        public string UserName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = "/assets/blank-profile-picture.png";
		public string? BackgroundUrl { get; set; } = "/assets/prostokat.png";
        public string? Description { get; set; } = "Hej, jestem użytkownikiem Luxy";
        public bool IsCurrentUserProfile { get; set; } = false;
        public bool IsFollowing { get; set; } = false;
        public List<FollowModel> PendingFollowRequests { get; set; } = new List<FollowModel>(); //nie dziala
    }
}
