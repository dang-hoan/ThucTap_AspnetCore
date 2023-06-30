using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnAspNetCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameOfCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Categories",
                newName: "Name");
        }
    }
}
