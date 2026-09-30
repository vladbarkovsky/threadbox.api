using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.ORM.Migrations
{
    /// <inheritdoc />
    public partial class BffSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs");

            migrationBuilder.CreateTable(
                name: "BffSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    AccessTokenExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    SessionExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BffSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BffSessions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BffSessions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BffSessions_CreatedById",
                table: "BffSessions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BffSessions_UpdatedById",
                table: "BffSessions",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BffSessions");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
