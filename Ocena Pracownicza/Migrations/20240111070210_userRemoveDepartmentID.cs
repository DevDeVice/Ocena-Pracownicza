﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ocena_Pracownicza.Migrations
{
    /// <inheritdoc />
    public partial class userRemoveDepartmentID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
