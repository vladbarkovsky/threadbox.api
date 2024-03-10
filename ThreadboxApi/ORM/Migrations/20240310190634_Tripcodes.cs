using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.ORM.Migrations
{
    /// <inheritdoc />
    public partial class Tripcodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TripcodeId",
                table: "Threads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Posts",
                type: "character varying(131072)",
                maxLength: 131072,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(131072)",
                oldMaxLength: 131072,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TripcodeId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tripcodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", maxLength: 16, nullable: false),
                    Hash = table.Column<byte[]>(type: "bytea", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tripcodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tripcodes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tripcodes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Tripcodes_CreatedById",
                table: "Tripcodes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tripcodes_Key",
                table: "Tripcodes",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_Tripcodes_UpdatedById",
                table: "Tripcodes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Tripcodes_TripcodeId",
                table: "Posts",
                column: "TripcodeId",
                principalTable: "Tripcodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Tripcodes_TripcodeId",
                table: "Threads",
                column: "TripcodeId",
                principalTable: "Tripcodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Tripcodes_TripcodeId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Tripcodes_TripcodeId",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "Tripcodes");

            migrationBuilder.DropIndex(
                name: "IX_Threads_TripcodeId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TripcodeId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TripcodeId",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "TripcodeId",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Posts",
                type: "character varying(131072)",
                maxLength: 131072,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(131072)",
                oldMaxLength: 131072);
        }
    }
}
