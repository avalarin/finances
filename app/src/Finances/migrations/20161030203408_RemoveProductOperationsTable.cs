using Microsoft.EntityFrameworkCore.Migrations;

namespace Finances.Migrations {
    public partial class RemoveProductOperationsTable : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OriginalAmount",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OriginalCurrency",
                table: "Operations");

            migrationBuilder.DropTable(
                name: "ProductOperations");

            migrationBuilder.AddColumn<decimal>(
                name: "Count",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Operations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ProductId",
                table: "Operations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Products_ProductId",
                table: "Operations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Products_ProductId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_ProductId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Operations");

            migrationBuilder.CreateTable(
                name: "ProductOperations",
                columns: table => new {
                    Id = table.Column<int>(nullable: false),
                    Count = table.Column<decimal>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ProductOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOperations_Operations_Id",
                        column: x => x.Id,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductOperations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalAmount",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalCurrency",
                table: "Operations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOperations_Id",
                table: "ProductOperations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOperations_OperationId",
                table: "ProductOperations",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOperations_ProductId",
                table: "ProductOperations",
                column: "ProductId");
        }
    }
}
