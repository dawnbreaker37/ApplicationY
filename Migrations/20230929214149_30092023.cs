using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _30092023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceOfProject",
                table: "Projects",
                newName: "TargetPrice");

            migrationBuilder.AddColumn<int>(
                name: "PastTargetPrice",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PastTargetPrice",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "TargetPrice",
                table: "Projects",
                newName: "PriceOfProject");
        }
    }
}
