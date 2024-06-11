namespace Luxa.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string TagName { get; set; } = default!;
        public ICollection<PhotoTagModel> PhotoTags { get; set; } = [];
    }
}
