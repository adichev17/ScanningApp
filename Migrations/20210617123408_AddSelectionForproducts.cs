using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScanningProductsApp.Migrations
{
    public partial class AddSelectionForproducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectionForProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    AdjacentID = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionForProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectionForProducts_ProductTable_AdjacentID",
                        column: x => x.AdjacentID,
                        principalTable: "ProductTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectionForProducts_ProductTable_ProductID",
                        column: x => x.ProductID,
                        principalTable: "ProductTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectionForProducts_AdjacentID",
                table: "SelectionForProducts",
                column: "AdjacentID");

            migrationBuilder.CreateIndex(
                name: "IX_SelectionForProducts_ProductID",
                table: "SelectionForProducts",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectionForProducts");
        }
    }
}
