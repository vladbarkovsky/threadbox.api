using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.Migrations
{
    /// <inheritdoc />
    public partial class ThreadboxFilesFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "ThreadImages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ThreadImages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "PostImages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PostImages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PostImages");
        }
    }
}
