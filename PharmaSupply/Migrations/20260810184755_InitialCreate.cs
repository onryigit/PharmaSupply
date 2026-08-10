using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaSupply.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsLicenseActive = table.Column<bool>(type: "bit", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyRedPrescriptionQuota = table.Column<int>(type: "int", nullable: false),
                    UsedRedPrescriptionQuota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(140)", maxLength: 140, nullable: false),
                    ActiveIngredient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrescriptionType = table.Column<int>(type: "int", nullable: false),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    IsBestSeller = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PharmacyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "medication", "Ağrı Kesiciler" },
                    { 2, "nutrition", "Vitamin & Takviye" },
                    { 3, "dermatology", "Dermokozmetik" },
                    { 4, "health_and_safety", "Medikal Ürünler" }
                });

            migrationBuilder.InsertData(
                table: "Pharmacies",
                columns: new[] { "Id", "Balance", "CreditLimit", "IsLicenseActive", "LicenseNumber", "MonthlyRedPrescriptionQuota", "Name", "UsedRedPrescriptionQuota" },
                values: new object[] { 1, 12500m, 75000m, true, "ECZ-34001", 100, "Şifa Eczanesi", 0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ActiveIngredient", "CategoryId", "Description", "ExpirationDate", "ImageUrl", "IsBestSeller", "Kind", "Name", "PrescriptionType", "Stock", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "Parasetamol", 1, "Paraset 500 mg, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=Paraset%20500%20mg", true, 0, "Paraset 500 mg", 0, 240, 68.50m },
                    { 2, "İbuprofen", 1, "İbucold Plus, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=%C4%B0bucold%20Plus", true, 0, "İbucold Plus", 0, 42, 124.90m },
                    { 3, "Omega-3", 2, "Omega 3 Forte, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=Omega%203%20Forte", true, 1, "Omega 3 Forte", 0, 85, 289.00m },
                    { 4, "Dekspantenol", 3, "Dermacare Krem, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=Dermacare%20Krem", false, 0, "Dermacare Krem", 0, 33, 176.50m },
                    { 5, "Diazepam", 1, "Diazepam 5 mg, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=Diazepam%205%20mg", false, 0, "Diazepam 5 mg", 2, 80, 92.25m },
                    { 6, "Morfin", 4, "Morfin Ampul, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://placehold.co/640x520/f0f6ff/0047ab?text=Morfin%20Ampul", false, 0, "Morfin Ampul", 1, 65, 440.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PharmacyId",
                table: "Orders",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotifications");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
