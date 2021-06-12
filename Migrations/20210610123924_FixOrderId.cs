using Microsoft.EntityFrameworkCore.Migrations;

namespace ScanningProductsApp.Migrations
{
    public partial class FixOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersTable_HistoryOrders_ProductId",
                table: "OrdersTable");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersTable_OrderId",
                table: "OrdersTable",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersTable_HistoryOrders_OrderId",
                table: "OrdersTable",
                column: "OrderId",
                principalTable: "HistoryOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersTable_HistoryOrders_OrderId",
                table: "OrdersTable");

            migrationBuilder.DropIndex(
                name: "IX_OrdersTable_OrderId",
                table: "OrdersTable");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersTable_HistoryOrders_ProductId",
                table: "OrdersTable",
                column: "ProductId",
                principalTable: "HistoryOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
