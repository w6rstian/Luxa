namespace Luxa.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int PhotoId { get; set; }
        public UserModel Owner { get; set; }
        public UserModel UserModelId { get; set; }

        public Photo Photo { get; set; }
    }
}

