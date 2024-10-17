using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMangementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FifthMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Librarians_LibrarianID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LibrarianID",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LibrarianID",
                table: "Loans");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Loans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReturnDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibrarianID",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LibrarianID",
                table: "Loans",
                column: "LibrarianID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Librarians_LibrarianID",
                table: "Loans",
                column: "LibrarianID",
                principalTable: "Librarians",
                principalColumn: "LibrarianID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
