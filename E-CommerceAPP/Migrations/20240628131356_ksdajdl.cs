using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPP.Migrations
{
    /// <inheritdoc />
    public partial class ksdajdl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_Customer_ID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Product_ID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Email_ID",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "Customer_ID",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email_ID",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Confirm_Password",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Customer_ID",
                table: "Products",
                column: "Customer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Customers_Customer_ID",
                table: "Products",
                column: "Customer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_Customer_ID",
                table: "Reviews",
                column: "Customer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Product_ID",
                table: "Reviews",
                column: "Product_ID",
                principalTable: "Products",
                principalColumn: "Product_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Customers_Customer_ID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Customers_Customer_ID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_Product_ID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Products_Customer_ID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Customer_ID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Confirm_Password",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email_ID",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email_ID",
                table: "Customers",
                column: "Email_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Customers_Customer_ID",
                table: "Reviews",
                column: "Customer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_Product_ID",
                table: "Reviews",
                column: "Product_ID",
                principalTable: "Products",
                principalColumn: "Product_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
