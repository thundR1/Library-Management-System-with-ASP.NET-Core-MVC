using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMangementSystem.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
    name: "PenaltyCost",
    table: "Loans",
    type: "int",
    nullable: false,
    defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
    name: "PenaltyCost",
    table: "Loans");
        }
    }
}
