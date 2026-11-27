using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.ORM.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDbFileAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_AspNetUsers_CreatedById",
                table: "DbFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_AspNetUsers_UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropIndex(
                name: "IX_DbFiles_CreatedById",
                table: "DbFiles");

            migrationBuilder.DropIndex(
                name: "IX_DbFiles_UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "DbFiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "DbFiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "DbFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "DbFiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "DbFiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "DbFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "DbFiles",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_DbFiles_CreatedById",
                table: "DbFiles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DbFiles_UpdatedById",
                table: "DbFiles",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DbFiles_AspNetUsers_CreatedById",
                table: "DbFiles",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DbFiles_AspNetUsers_UpdatedById",
                table: "DbFiles",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
