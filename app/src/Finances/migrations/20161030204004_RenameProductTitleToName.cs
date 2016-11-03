using Microsoft.EntityFrameworkCore.Migrations;

namespace Finances.Migrations {
    public partial class RenameProductTitleToName : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Products",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Products",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
