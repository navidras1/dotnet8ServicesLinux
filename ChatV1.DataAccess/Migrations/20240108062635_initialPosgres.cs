using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatV1.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initialPosgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRoomTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MaxMember = table.Column<int>(type: "integer", nullable: true),
                    Istemp = table.Column<bool>(type: "boolean", nullable: true),
                    IsChannel = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatStatus1 = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    LastSeenDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogRequestResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    ControllerName = table.Column<string>(type: "text", nullable: true),
                    Request = table.Column<string>(type: "text", nullable: true),
                    Response = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRequestResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ChatRoomTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatorUserName = table.Column<string>(type: "text", nullable: true),
                    CreateDatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRooms_ChatRoomTypes_ChatRoomTypeId",
                        column: x => x.ChatRoomTypeId,
                        principalTable: "ChatRoomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromEmpId = table.Column<int>(type: "integer", nullable: false),
                    ToEmPid = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    ChatStatusId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChatGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    FromUserName = table.Column<string>(type: "text", nullable: true),
                    ToUserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatLogs_ChatStatuses_ChatStatusId",
                        column: x => x.ChatStatusId,
                        principalTable: "ChatStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContancts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmpMasterId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContancts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContancts_EmpMasters_EmpMasterId",
                        column: x => x.EmpMasterId,
                        principalTable: "EmpMasters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomMemebers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatRoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomMemebers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRoomMemebers_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatRoomLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChatLogId = table.Column<long>(type: "bigint", nullable: false),
                    ChatRoomId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRoomLogs_ChatLogs_ChatLogId",
                        column: x => x.ChatLogId,
                        principalTable: "ChatLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomLogs_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatLogs_ChatStatusId",
                table: "ChatLogs",
                column: "ChatStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomLogs_ChatLogId",
                table: "ChatRoomLogs",
                column: "ChatLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomLogs_ChatRoomId",
                table: "ChatRoomLogs",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomMemebers_ChatRoomId",
                table: "ChatRoomMemebers",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_ChatRoomTypeId",
                table: "ChatRooms",
                column: "ChatRoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContancts_EmpMasterId",
                table: "UserContancts",
                column: "EmpMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomLogs");

            migrationBuilder.DropTable(
                name: "ChatRoomMemebers");

            migrationBuilder.DropTable(
                name: "LogRequestResponses");

            migrationBuilder.DropTable(
                name: "UserContancts");

            migrationBuilder.DropTable(
                name: "ChatLogs");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "EmpMasters");

            migrationBuilder.DropTable(
                name: "ChatStatuses");

            migrationBuilder.DropTable(
                name: "ChatRoomTypes");
        }
    }
}
