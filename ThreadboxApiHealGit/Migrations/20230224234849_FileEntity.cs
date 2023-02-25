using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.Migrations
{
    /// <inheritdoc />
    public partial class FileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ThreadImages",
                newName: "File_Name");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "ThreadImages",
                newName: "File_Extension");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "ThreadImages",
                newName: "File_Data");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "ThreadImages",
                newName: "File_ContentType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PostImages",
                newName: "File_Name");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "PostImages",
                newName: "File_Extension");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "PostImages",
                newName: "File_Data");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "PostImages",
                newName: "File_ContentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "File_Name",
                table: "ThreadImages",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "File_Extension",
                table: "ThreadImages",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "File_Data",
                table: "ThreadImages",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "File_ContentType",
                table: "ThreadImages",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "File_Name",
                table: "PostImages",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "File_Extension",
                table: "PostImages",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "File_Data",
                table: "PostImages",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "File_ContentType",
                table: "PostImages",
                newName: "ContentType");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
