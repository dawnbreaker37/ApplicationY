﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationY.Migrations
{
    /// <inheritdoc />
    public partial class _291120230 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Audios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Audios");
        }
    }
}
