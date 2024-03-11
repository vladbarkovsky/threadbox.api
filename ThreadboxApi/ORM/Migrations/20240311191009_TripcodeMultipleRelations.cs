using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.ORM.Migrations
{
    /// <inheritdoc />
    public partial class TripcodeMultipleRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Threads_TripcodeId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TripcodeId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_TripcodeId",
                table: "Threads",
                column: "TripcodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TripcodeId",
                table: "Posts",
                column: "TripcodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Threads_TripcodeId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TripcodeId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_TripcodeId",
                table: "Threads",
                column: "TripcodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TripcodeId",
                table: "Posts",
                column: "TripcodeId",
                unique: true);
        }
    }
}
