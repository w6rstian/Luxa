namespace Luxa.ViewModel
{
    public class UserProfileVM
    {
        public string UserName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = "/assets/blank-profile-picture.png";
		public string? BackgroundUrl { get; set; } = "/assets/prostokat.png";
        public string? Description { get; set; } = "Hej, jestem użytkownikiem Luxy";
    }
}
