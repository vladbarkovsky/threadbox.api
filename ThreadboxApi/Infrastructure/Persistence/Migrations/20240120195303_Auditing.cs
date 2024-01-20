using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Auditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Persons",
                newName: "UpdatedById");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Threads",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Threads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Threads",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "Threads",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Threads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Threads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ThreadImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ThreadImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ThreadImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "ThreadImages",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ThreadImages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "ThreadImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RegistrationKeys",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "RegistrationKeys",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "RegistrationKeys",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RegistrationKeys",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "RegistrationKeys",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "Posts",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "PostImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "PostImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "PostImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "PostImages",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "PostImages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "PostImages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Persons",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Persons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Persons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "Persons",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Persons",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "FileInfos",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "FileInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "FileInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "FileInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "FileInfos",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "FileInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "FileInfos",
                type: "text",
                nullable: true);

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

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "DbFiles",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

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

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Boards",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Boards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Boards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "RowVersion",
                table: "Boards",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Boards",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Boards",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityPrimaryKeys = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "text", nullable: false),
                    Operation = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OldValues = table.Column<string>(type: "text", nullable: true),
                    NewValues = table.Column<string>(type: "text", nullable: true),
                    AffectedProperties = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CreatedById",
                table: "Threads",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_UpdatedById",
                table: "Threads",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadImages_CreatedById",
                table: "ThreadImages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadImages_UpdatedById",
                table: "ThreadImages",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationKeys_CreatedById",
                table: "RegistrationKeys",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationKeys_UpdatedById",
                table: "RegistrationKeys",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_CreatedById",
                table: "PostImages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_UpdatedById",
                table: "PostImages",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CreatedById",
                table: "Persons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UpdatedById",
                table: "Persons",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FileInfos_CreatedById",
                table: "FileInfos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FileInfos_UpdatedById",
                table: "FileInfos",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DbFiles_CreatedById",
                table: "DbFiles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DbFiles_UpdatedById",
                table: "DbFiles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_CreatedById",
                table: "Boards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_UpdatedById",
                table: "Boards",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_AspNetUsers_CreatedById",
                table: "Boards",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_AspNetUsers_UpdatedById",
                table: "Boards",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfos_AspNetUsers_CreatedById",
                table: "FileInfos",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfos_AspNetUsers_UpdatedById",
                table: "FileInfos",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_CreatedById",
                table: "Persons",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_AspNetUsers_UpdatedById",
                table: "Persons",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_AspNetUsers_CreatedById",
                table: "PostImages",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_AspNetUsers_UpdatedById",
                table: "PostImages",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationKeys_AspNetUsers_CreatedById",
                table: "RegistrationKeys",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationKeys_AspNetUsers_UpdatedById",
                table: "RegistrationKeys",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadImages_AspNetUsers_CreatedById",
                table: "ThreadImages",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadImages_AspNetUsers_UpdatedById",
                table: "ThreadImages",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_AspNetUsers_CreatedById",
                table: "Threads",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_AspNetUsers_UpdatedById",
                table: "Threads",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_AspNetUsers_CreatedById",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_AspNetUsers_UpdatedById",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_AspNetUsers_CreatedById",
                table: "DbFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_AspNetUsers_UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FileInfos_AspNetUsers_CreatedById",
                table: "FileInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_FileInfos_AspNetUsers_UpdatedById",
                table: "FileInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_CreatedById",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_AspNetUsers_UpdatedById",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_AspNetUsers_CreatedById",
                table: "PostImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_AspNetUsers_UpdatedById",
                table: "PostImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_CreatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UpdatedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationKeys_AspNetUsers_CreatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationKeys_AspNetUsers_UpdatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadImages_AspNetUsers_CreatedById",
                table: "ThreadImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadImages_AspNetUsers_UpdatedById",
                table: "ThreadImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_AspNetUsers_CreatedById",
                table: "Threads");

            migrationBuilder.DropForeignKey(
                name: "FK_Threads_AspNetUsers_UpdatedById",
                table: "Threads");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_Threads_CreatedById",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_UpdatedById",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_ThreadImages_CreatedById",
                table: "ThreadImages");

            migrationBuilder.DropIndex(
                name: "IX_ThreadImages_UpdatedById",
                table: "ThreadImages");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationKeys_CreatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropIndex(
                name: "IX_RegistrationKeys_UpdatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CreatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UpdatedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_CreatedById",
                table: "PostImages");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_UpdatedById",
                table: "PostImages");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CreatedById",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_UpdatedById",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_FileInfos_CreatedById",
                table: "FileInfos");

            migrationBuilder.DropIndex(
                name: "IX_FileInfos_UpdatedById",
                table: "FileInfos");

            migrationBuilder.DropIndex(
                name: "IX_DbFiles_CreatedById",
                table: "DbFiles");

            migrationBuilder.DropIndex(
                name: "IX_DbFiles_UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropIndex(
                name: "IX_Boards_CreatedById",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_UpdatedById",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "ThreadImages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "RegistrationKeys");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "RegistrationKeys");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RegistrationKeys");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "RegistrationKeys");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "PostImages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FileInfos");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "FileInfos");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "FileInfos");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FileInfos");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FileInfos");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "FileInfos");

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
                name: "RowVersion",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Boards");

            migrationBuilder.RenameColumn(
                name: "UpdatedById",
                table: "Persons",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "FileInfos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);
        }
    }
}
