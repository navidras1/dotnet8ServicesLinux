using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addClientDateTimeToChatLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClientDateTime",
                table: "ChatLogs",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientDateTime",
                table: "ChatLogs");
        }
    }
}
