using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class chatAttachmentModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatAttachment_ChatLogs_ChatLogId",
                table: "ChatAttachment");

            migrationBuilder.DropIndex(
                name: "IX_ChatAttachment_ChatLogId",
                table: "ChatAttachment");

            migrationBuilder.CreateTable(
                name: "ChatLogAttachment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatLogId = table.Column<long>(type: "bigint", nullable: false),
                    ChatAttachmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLogAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatLogAttachment_ChatAttachment_ChatAttachmentId",
                        column: x => x.ChatAttachmentId,
                        principalTable: "ChatAttachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatLogAttachment_ChatLogs_ChatLogId",
                        column: x => x.ChatLogId,
                        principalTable: "ChatLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatLogAttachment_ChatAttachmentId",
                table: "ChatLogAttachment",
                column: "ChatAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatLogAttachment_ChatLogId",
                table: "ChatLogAttachment",
                column: "ChatLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatLogAttachment");

            migrationBuilder.CreateIndex(
                name: "IX_ChatAttachment_ChatLogId",
                table: "ChatAttachment",
                column: "ChatLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatAttachment_ChatLogs_ChatLogId",
                table: "ChatAttachment",
                column: "ChatLogId",
                principalTable: "ChatLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
