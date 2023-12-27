using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Hotels",
                newName: "MainImageURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainImageURL",
                table: "Hotels",
                newName: "ImageURL");
        }
    }
}
