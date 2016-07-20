﻿using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Finances.Migrations {
    public partial class CreateCurrenciesTable : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Text = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Currencies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_BookId",
                table: "Currencies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code_BookId",
                table: "Currencies",
                columns: new[] { "Code", "BookId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
