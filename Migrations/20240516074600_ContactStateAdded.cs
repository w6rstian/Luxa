using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luxa.Migrations
{
    /// <inheritdoc />
    public partial class ContactStateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StateString",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "StateString",
                table: "Contacts");
        }
    }
}
