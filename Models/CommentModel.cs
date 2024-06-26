﻿namespace Luxa.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; } = default!;
        public int PhotoId { get; set; }
        public string OwnerId { get; set; } = default!;

		public Photo Photo { get; set; } = default!;
        public UserModel Owner { get; set; } = default!;
        //public DateTime AddTime { get; set; } = DateTime.Now;
    }
}
