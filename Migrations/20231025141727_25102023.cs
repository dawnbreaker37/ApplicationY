using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _25102023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Messages_MessageId",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Replies",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_MessageId",
                table: "Replies",
                newName: "IX_Replies_CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Comments_CommentId",
                table: "Replies");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Replies",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Replies_CommentId",
                table: "Replies",
                newName: "IX_Replies_MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Messages_MessageId",
                table: "Replies",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");
        }
    }
}
