using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMangementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigrat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Librarians",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Members_IdentityUserId",
                table: "Members",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Librarians_IdentityUserId",
                table: "Librarians",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_AspNetUsers_IdentityUserId",
                table: "Librarians",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_IdentityUserId",
                table: "Members",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_AspNetUsers_IdentityUserId",
                table: "Librarians");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_IdentityUserId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_IdentityUserId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Librarians_IdentityUserId",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Librarians");
        }
    }
}
