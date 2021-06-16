using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kwetter.FollowingService.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocked",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BlockedId = table.Column<int>(type: "int", nullable: false),
                    BlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocked", x => new { x.UserId, x.BlockedId });
                });

            migrationBuilder.CreateTable(
                name: "ProfileReferences",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileReferences", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Followings",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FollowingId = table.Column<int>(type: "int", nullable: false),
                    UserProfileUserId = table.Column<int>(type: "int", nullable: true),
                    FollowingProfileUserId = table.Column<int>(type: "int", nullable: true),
                    FollowingSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followings", x => new { x.UserId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_Followings_ProfileReferences_FollowingProfileUserId",
                        column: x => x.FollowingProfileUserId,
                        principalTable: "ProfileReferences",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Followings_ProfileReferences_UserProfileUserId",
                        column: x => x.UserProfileUserId,
                        principalTable: "ProfileReferences",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followings_FollowingProfileUserId",
                table: "Followings",
                column: "FollowingProfileUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_UserProfileUserId",
                table: "Followings",
                column: "UserProfileUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocked");

            migrationBuilder.DropTable(
                name: "Followings");

            migrationBuilder.DropTable(
                name: "ProfileReferences");
        }
    }
}
