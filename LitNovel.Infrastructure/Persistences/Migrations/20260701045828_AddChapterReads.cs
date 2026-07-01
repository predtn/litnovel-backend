using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LitNovel.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddChapterReads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChapterReads",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: false),
                    NovelId = table.Column<int>(type: "int", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterReads", x => new { x.UserId, x.ChapterId });
                    table.ForeignKey(
                        name: "FK_ChapterReads_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterReads_Novels_NovelId",
                        column: x => x.NovelId,
                        principalTable: "Novels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterReads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterReads_ChapterId",
                table: "ChapterReads",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterReads_NovelId",
                table: "ChapterReads",
                column: "NovelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterReads_UserId_NovelId",
                table: "ChapterReads",
                columns: new[] { "UserId", "NovelId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterReads");
        }
    }
}
