using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUserChatRoomRecieverAndJoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserChatRoomRecievers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ChatLogId = table.Column<long>(type: "bigint", nullable: false),
                    ChatStatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatRoomRecievers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChatRoomRecievers_ChatLogs_ChatLogId",
                        column: x => x.ChatLogId,
                        principalTable: "ChatLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChatRoomRecievers_ChatStatuses_ChatStatusId",
                        column: x => x.ChatStatusId,
                        principalTable: "ChatStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChatRoomRecievers_ChatLogId",
                table: "UserChatRoomRecievers",
                column: "ChatLogId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChatRoomRecievers_ChatStatusId",
                table: "UserChatRoomRecievers",
                column: "ChatStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChatRoomRecievers");
        }
    }
}
