using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedThreddIndextoChatLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChatLogs_FromUserName_ToUserName_ChatGuid",
                table: "ChatLogs",
                columns: new[] { "FromUserName", "ToUserName", "ChatGuid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatLogs_FromUserName_ToUserName_ChatGuid",
                table: "ChatLogs");
        }
    }
}
