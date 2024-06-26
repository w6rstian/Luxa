﻿using Luxa.Data.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Luxa.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [JsonIgnore]
        public UserModel Owner { get; set; } = default!;

        public string OwnerId { get; set; } = default!;

        [DisplayName("Podaj nazwę zdjęcia")]
        public string Name { get; set; } = default!;

        [DisplayName("Podaj opis zdjęcia")]
        public string Description { get; set; } = default!;

        [DisplayName("Wybierz kategorię")]
        [EnumDataType(typeof(CategoryOfPhotos))]
        public CategoryOfPhotos Category { get; set; } = CategoryOfPhotos.None;

        public DateTime AddTime { get; set; } = DateTime.Now;

        public int Views { get; set; } = 0;

        public int LikeCount { get; set; } = 0;

        [NotMapped]
        [DisplayName("Zaladuj zdjecie")]
        public IFormFile ImageFile { get; set; } = default!;

        public ICollection<UserPhotoModel> UserLikedPhotos { get; set; } = new List<UserPhotoModel>();

        public ICollection<PhotoTagModel> PhotoTags { get; set; } = new List<PhotoTagModel>();

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}
