﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrof.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseStateUpdate20230710 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[] { 100, 100, "Accessories" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 100);
        }
    }
}
