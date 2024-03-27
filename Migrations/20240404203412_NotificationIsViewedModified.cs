using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luxa.Migrations
{
    /// <inheritdoc />
    public partial class NotificationIsViewedModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserNotifications");

            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "UserNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "UserNotifications");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
