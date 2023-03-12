﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThreadboxApi.Migrations
{
    /// <inheritdoc />
    public partial class RegistrationKeyToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "RegistrationKeys");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Value",
                table: "RegistrationKeys",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
