namespace Luxa.Models
{
    public class FollowModel
    {
        public int Id { get; set; }
        public string? FollowerId { get; set; }
        public UserModel? Follower { get; set; }
        public string? FolloweeId { get; set; }
        public UserModel? Followee { get; set; }
        public bool IsApproved { get; set; }
        //public bool IsMutual { get; set; }
    }
}
