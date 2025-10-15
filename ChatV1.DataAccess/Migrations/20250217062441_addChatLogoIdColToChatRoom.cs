using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addChatLogoIdColToChatRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatRoomLogoId",
                table: "ChatRooms",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoomLogoId",
                table: "ChatRooms");
        }
    }
}
