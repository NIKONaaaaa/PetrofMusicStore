using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petrof.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseStateWithInStockCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InStock",
                table: "Products");
        }
    }
}
