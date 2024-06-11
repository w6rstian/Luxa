namespace Luxa.Models
{
    public class UserPhotoModel
    {
        public string UserId { get; set; } = default!;
        public UserModel User { get; set; } = default!;

        public int PhotoId { get; set; } = default!;
        public Photo Photo { get; set; } = default!;
    }
}
