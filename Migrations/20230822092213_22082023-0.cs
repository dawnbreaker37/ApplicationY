using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _220820230 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PseudoName",
                table: "AspNetUsers",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PseudoName",
                table: "AspNetUsers");
        }
    }
}
