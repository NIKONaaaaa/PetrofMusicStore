using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrof.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _20230713AddSessionIdToOrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomSessionId",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomSessionId",
                table: "OrderHeaders");
        }
    }
}
