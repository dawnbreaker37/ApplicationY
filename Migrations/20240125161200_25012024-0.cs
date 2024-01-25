using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _250120240 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisabledAccounts_AspNetUsers_UsertId",
                table: "DisabledAccounts");

            migrationBuilder.RenameColumn(
                name: "UsertId",
                table: "DisabledAccounts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DisabledAccounts_UsertId",
                table: "DisabledAccounts",
                newName: "IX_DisabledAccounts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisabledAccounts_AspNetUsers_UserId",
                table: "DisabledAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisabledAccounts_AspNetUsers_UserId",
                table: "DisabledAccounts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DisabledAccounts",
                newName: "UsertId");

            migrationBuilder.RenameIndex(
                name: "IX_DisabledAccounts_UserId",
                table: "DisabledAccounts",
                newName: "IX_DisabledAccounts_UsertId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisabledAccounts_AspNetUsers_UsertId",
                table: "DisabledAccounts",
                column: "UsertId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
