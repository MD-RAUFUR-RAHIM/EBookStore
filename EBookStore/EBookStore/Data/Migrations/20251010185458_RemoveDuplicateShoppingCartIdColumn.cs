using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBookStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateShoppingCartIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoopingCartId",
                table: "CartDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoopingCartId",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
