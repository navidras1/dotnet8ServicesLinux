using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addReplyOfGuidToChatLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyOfGuid",
                table: "ChatLogs",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyOfGuid",
                table: "ChatLogs");
        }
    }
}
