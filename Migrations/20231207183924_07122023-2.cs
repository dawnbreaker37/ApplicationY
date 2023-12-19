using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _071220232 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoUrl",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "MainPhotoId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "MainPhotoUrl",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
