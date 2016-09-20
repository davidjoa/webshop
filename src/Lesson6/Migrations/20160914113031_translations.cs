using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lesson6.Migrations
{
    public partial class translations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTranslations",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    ProductDescription = table.Column<string>(nullable: false),
                    ProductName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslations", x => new { x.ProductId, x.Language });
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategoryName",
                table: "ProductCategories",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Products",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_ProductId",
                table: "ProductTranslations",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategoryName",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Products",
                nullable: true);
        }
    }
}
