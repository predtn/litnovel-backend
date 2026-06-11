using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LitNovel.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddNovelAndCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CommentChapterId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => new { x.UserId, x.CommentChapterId });
                    table.ForeignKey(
                        name: "FK_CommentLikes_CommentChapters_CommentChapterId",
                        column: x => x.CommentChapterId,
                        principalTable: "CommentChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NovelLikes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NovelId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelLikes", x => new { x.UserId, x.NovelId });
                    table.ForeignKey(
                        name: "FK_NovelLikes_Novels_NovelId",
                        column: x => x.NovelId,
                        principalTable: "Novels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NovelLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentChapterId",
                table: "CommentLikes",
                column: "CommentChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_NovelLikes_NovelId",
                table: "NovelLikes",
                column: "NovelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "NovelLikes");
        }
    }
}
