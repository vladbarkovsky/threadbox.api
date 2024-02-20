using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.ORM.Migrations
{
    /// <inheritdoc />
    public partial class SimplifiedFileInfoRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThreadImages_FileInfoId",
                table: "ThreadImages");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_FileInfoId",
                table: "PostImages");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadImages_FileInfoId",
                table: "ThreadImages",
                column: "FileInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_FileInfoId",
                table: "PostImages",
                column: "FileInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThreadImages_FileInfoId",
                table: "ThreadImages");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_FileInfoId",
                table: "PostImages");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadImages_FileInfoId",
                table: "ThreadImages",
                column: "FileInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_FileInfoId",
                table: "PostImages",
                column: "FileInfoId");
        }
    }
}
