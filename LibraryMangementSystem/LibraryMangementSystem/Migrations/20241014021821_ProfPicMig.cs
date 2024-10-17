using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMangementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ProfPicMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Members",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Members");
        }
    }
}
