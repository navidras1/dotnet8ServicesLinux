using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removedChatlogIdFromChatAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatLogId",
                table: "ChatAttachment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatLogId",
                table: "ChatAttachment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
