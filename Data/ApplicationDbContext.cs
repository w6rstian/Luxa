﻿using Luxa.Models;
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


			modelBuilder.Entity<UserPhotoModel>()
				.HasOne(e => e.User)
				.WithMany(e => e.UserLikedPhotos)
				.HasForeignKey(e => e.UserId);
			modelBuilder.Entity<UserPhotoModel>()
				.HasOne(e => e.Photo)
				.WithMany(e => e.UserLikedPhotos)
				.HasForeignKey(e => e.PhotoId);
			modelBuilder.Entity<UserPhotoModel>()
				.HasKey(un => new { un.UserId, un.PhotoId });



			modelBuilder.Entity<IdentityUserLogin<string>>()
		.HasKey(l => new { l.LoginProvider, l.ProviderKey });
			modelBuilder.Entity<IdentityUserRole<string>>()
				.HasKey(r => new { r.UserId, r.RoleId });
			modelBuilder.Entity<IdentityUserToken<string>>()
				.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

		}

		public DbSet<UserNotificationModel> UserNotifications { get; set; }
		public DbSet<UserPhotoModel> UserLikedPhotos { get; set; }
		public DbSet<NotificationModel> Notifications { get; set; }
		public DbSet<Photo> Photo { get; set; } = default!;
		public DbSet<ContactModel> Contacts { get; set; }


	}
}
