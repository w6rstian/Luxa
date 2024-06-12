using Luxa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Luxa.Data
{
	public class ApplicationDbContext : IdentityDbContext<UserModel>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//wiele do wielu - powiadomienia/użytkownicy
			modelBuilder.Entity<UserNotificationModel>()
				.HasOne(e => e.User)
				.WithMany(e => e.UserNotifiacations)
				.HasForeignKey(e => e.UserId);
			modelBuilder.Entity<UserNotificationModel>()
				.HasOne(e => e.Notification)
				.WithMany(e => e.UserNotifications)
				.HasForeignKey(e => e.NotificationId);
			modelBuilder.Entity<UserNotificationModel>()
				.HasKey(un => new { un.UserId, un.NotificationId });

			//wiele do wielu - zdjęcia/użytkownicy - polubione
			modelBuilder.Entity<UserPhotoModel>()
				.HasOne(e => e.User)
				.WithMany(e => e.UserLikedPhotos)
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<UserPhotoModel>()
				.HasOne(e => e.Photo)
				.WithMany(e => e.UserLikedPhotos)
				.HasForeignKey(e => e.PhotoId)
				.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<UserPhotoModel>()
				.HasKey(un => new { un.UserId, un.PhotoId });
			//wiele do wielu - zdjęcia/tagi
			modelBuilder.Entity<PhotoTagModel>()
				.HasOne(e => e.Tag)
				.WithMany(e => e.PhotoTags)
				.HasForeignKey(e => e.TagId);
			modelBuilder.Entity<PhotoTagModel>()
				.HasOne(e => e.Photo)
				.WithMany(e => e.PhotoTags)
				.HasForeignKey(e => e.PhotoId);
			modelBuilder.Entity<PhotoTagModel>()
				.HasKey(un => new { un.TagId, un.PhotoId });

			//jeden do wielu - użytkownik/zdjęcia
			modelBuilder.Entity<Photo>()
				.HasOne(p => p.Owner)
				.WithMany(u => u.Photos)
				.HasForeignKey(p => p.OwnerId)
				.OnDelete(DeleteBehavior.Restrict);


			//wiele do wielu - zdjęcia/użytkownicy - komentarze
			modelBuilder.Entity<CommentModel>()
			.HasOne(e => e.Owner)
			.WithMany(e => e.Comments)
			.HasForeignKey(e => e.OwnerId)
			.OnDelete(DeleteBehavior.Restrict);
			modelBuilder.Entity<CommentModel>()
				.HasOne(e => e.Photo)
				.WithMany(e => e.Comments)
				.HasForeignKey(e => e.PhotoId)
			.OnDelete(DeleteBehavior.Restrict);





			//modelBuilder.Entity<UserModel>()
			//	.HasOne(p => p.ProfilePhoto)
			//	.WithOne(p => p.Owner)
			//	.OnDelete(DeleteBehavior.Cascade);

			// jeden do wielu - zdjęcie/komentarz
			//modelBuilder.Entity<CommentModel>()
			//.HasOne(c => c.Photo)
			//.WithMany(p => p.Comments)
			//.HasForeignKey(c => c.PhotoId)
			//.OnDelete(DeleteBehavior.Cascade);




			modelBuilder.Entity<IdentityUserLogin<string>>()
		.HasKey(l => new { l.LoginProvider, l.ProviderKey });
			modelBuilder.Entity<IdentityUserRole<string>>()
				.HasKey(r => new { r.UserId, r.RoleId });
			modelBuilder.Entity<IdentityUserToken<string>>()
				.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

			//obserwujący
			modelBuilder.Entity<FollowModel>()
			   .HasOne(f => f.Follower)
			   .WithMany(u => u.Following)
			   .HasForeignKey(f => f.FollowerId)
			   .OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<FollowModel>()
				.HasOne(f => f.Followee)
				.WithMany(u => u.Followers)
				.HasForeignKey(f => f.FolloweeId)
				.OnDelete(DeleteBehavior.Restrict);

			/*            modelBuilder.Entity<FollowModel>()
							.HasOne(f => f.Follower)
							.WithMany(u => u.MutualFollowers)
							.HasForeignKey(f => f.FollowerId)
							.OnDelete(DeleteBehavior.Restrict);*/
		}

		public DbSet<UserNotificationModel> UserNotifications { get; set; }
		public DbSet<UserPhotoModel> UserLikedPhotos { get; set; }
		public DbSet<NotificationModel> Notifications { get; set; }
		public DbSet<Photo> Photo { get; set; }
		public DbSet<ContactModel> Contacts { get; set; }
		public DbSet<PhotoTagModel> PhotoTags { get; set; }
		public DbSet<TagModel> Tags { get; set; }
		public DbSet<FollowModel> FollowRequests { get; set; }
		public DbSet<CommentModel> Comments { get; set; }
		//public DbSet<UserModel> Users { get; set; }

	}
}