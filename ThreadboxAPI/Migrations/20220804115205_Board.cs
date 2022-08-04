using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxAPI.Migrations
{
    public partial class Board : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Sections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_BoardId",
                table: "Sections",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Board_BoardId",
                table: "Sections",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Board_BoardId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Sections_BoardId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Sections");
        }
    }
}
