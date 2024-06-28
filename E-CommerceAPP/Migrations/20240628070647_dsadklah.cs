using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPP.Migrations
{
    /// <inheritdoc />
    public partial class dsadklah : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(name: "Category_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(name: "Category_Name", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(name: "Customer_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(name: "First_Name", type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(name: "Last_Name", type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<long>(name: "Contact_Number", type: "bigint", nullable: false),
                    EmailID = table.Column<string>(name: "Email_ID", type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(name: "Product_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(name: "Product_Name", type: "nvarchar(max)", nullable: false),
                    ProductDescription = table.Column<string>(name: "Product_Description", type: "nvarchar(max)", nullable: false),
                    ProductPrice = table.Column<decimal>(name: "Product_Price", type: "decimal(18,2)", nullable: false),
                    CategoryID = table.Column<int>(name: "Category_ID", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Categories_Category_ID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(name: "Review_ID", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductRating = table.Column<int>(name: "Product_Rating", type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductID = table.Column<int>(name: "Product_ID", type: "int", nullable: false),
                    CustomerID = table.Column<int>(name: "Customer_ID", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_Reviews_Customers_Customer_ID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "Customer_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_Product_ID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email_ID",
                table: "Customers",
                column: "Email_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category_ID",
                table: "Products",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Customer_ID",
                table: "Reviews",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Product_ID",
                table: "Reviews",
                column: "Product_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
