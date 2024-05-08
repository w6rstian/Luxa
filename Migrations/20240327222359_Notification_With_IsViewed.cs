using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luxa.Migrations
{
    /// <inheritdoc />
    public partial class Notification_With_IsViewed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsViewed",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsViewed",
                table: "Notifications");
        }
    }
}
