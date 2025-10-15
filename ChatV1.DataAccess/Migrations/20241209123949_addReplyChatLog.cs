using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addReplyChatLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyOfGuid",
                table: "ChatLogs");

            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "ChatLogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reply",
                table: "ChatLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "ReplyOfGuid",
                table: "ChatLogs",
                type: "uuid",
                nullable: true);
        }
    }
}
