using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPP.Migrations
{
    /// <inheritdoc />
    public partial class sdabjdba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Customers_Customer_ID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Customer_ID",
                table: "Products",
                newName: "CustomersCustomer_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Customer_ID",
                table: "Products",
                newName: "IX_Products_CustomersCustomer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Customers_CustomersCustomer_ID",
                table: "Products",
                column: "CustomersCustomer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Customers_CustomersCustomer_ID",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CustomersCustomer_ID",
                table: "Products",
                newName: "Customer_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CustomersCustomer_ID",
                table: "Products",
                newName: "IX_Products_Customer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Customers_Customer_ID",
                table: "Products",
                column: "Customer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
