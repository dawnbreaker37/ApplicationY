using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _260820230 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "TemporaryCodes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "TemporaryCodes");
        }
    }
}
