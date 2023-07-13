using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrof.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _20230713AddSessionIdToOrderHeader1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomSessionId",
                table: "OrderHeaders",
                newName: "ShopSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShopSessionId",
                table: "OrderHeaders",
                newName: "CustomSessionId");
        }
    }
}
