using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LitNovel.Infrastructure.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddNovelPreviousPublicStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousPublicStatus",
                table: "Novels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousPublicStatus",
                table: "Novels");
        }
    }
}
