using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LitNovel.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingDeletionLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionRequestedAt",
                table: "Novels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletionRequestedById",
                table: "Novels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledHardDeleteAt",
                table: "Novels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionRequestedAt",
                table: "Chapters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletionRequestedById",
                table: "Chapters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPublicStatus",
                table: "Chapters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledHardDeleteAt",
                table: "Chapters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Novels_ScheduledHardDeleteAt",
                table: "Novels",
                column: "ScheduledHardDeleteAt");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ScheduledHardDeleteAt",
                table: "Chapters",
                column: "ScheduledHardDeleteAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Novels_ScheduledHardDeleteAt",
                table: "Novels");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_ScheduledHardDeleteAt",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "DeletionRequestedAt",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "DeletionRequestedById",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "ScheduledHardDeleteAt",
                table: "Novels");

            migrationBuilder.DropColumn(
                name: "DeletionRequestedAt",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "DeletionRequestedById",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "PreviousPublicStatus",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "ScheduledHardDeleteAt",
                table: "Chapters");
        }
    }
}
