using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceAPP.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addrees",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pincode = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addrees", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "deliverylists",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estimateddeliver = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deliverydate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shipmentid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverylists", x => x.DeliveryId);
                });

            migrationBuilder.CreateTable(
                name: "shipment",
                columns: table => new
                {
                    shipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shipmentdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment", x => x.shipmentId);
                });

            migrationBuilder.CreateTable(
                name: "orderlists",
                columns: table => new
                {
                    OrderlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    AddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderlists", x => x.OrderlistId);
                    table.ForeignKey(
                        name: "FK_orderlists_addrees_AddressId",
                        column: x => x.AddressId,
                        principalTable: "addrees",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderlists_shipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "shipment",
                        principalColumn: "shipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paymentlists",
                columns: table => new
                {
                    paymentId = table.Column<int>(type: "int", nullable: false),
                    paymentdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    paymenttype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paymentstatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentlists", x => x.paymentId);
                    table.ForeignKey(
                        name: "FK_paymentlists_orderlists_paymentId",
                        column: x => x.paymentId,
                        principalTable: "orderlists",
                        principalColumn: "OrderlistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderlists_AddressId",
                table: "orderlists",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderlists_ShipmentId",
                table: "orderlists",
                column: "ShipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deliverylists");

            migrationBuilder.DropTable(
                name: "paymentlists");

            migrationBuilder.DropTable(
                name: "orderlists");

            migrationBuilder.DropTable(
                name: "addrees");

            migrationBuilder.DropTable(
                name: "shipment");
        }
    }
}
