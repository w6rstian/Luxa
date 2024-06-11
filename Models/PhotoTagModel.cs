namespace Luxa.Models
{
    public class PhotoTagModel
    {
        public int PhotoId { get; set; }
        public Photo Photo { get; set; } = default!;
        public int TagId { get; set; }
        public TagModel Tag { get; set; } = default!;
    }
}
