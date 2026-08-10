using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaSupply.Migrations
{
    /// <inheritdoc />
    public partial class ExpandCatalogAndImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/products/analgesics.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/products/analgesics.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/products/supplements.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/products/dermocosmetics.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/products/analgesics.png");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/products/medical-supplies.png");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ActiveIngredient", "CategoryId", "Description", "ExpirationDate", "ImageUrl", "IsBestSeller", "Kind", "Name", "PrescriptionType", "Stock", "UnitPrice" },
                values: new object[,]
                {
                    { 7, "Naproksen Sodyum", 1, "Naproksen Forte 550 mg, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/analgesics.png", true, 0, "Naproksen Forte 550 mg", 0, 118, 148.75m },
                    { 8, "Asetilsalisilik Asit", 1, "Migra Relief 250 mg, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/analgesics.png", false, 0, "Migra Relief 250 mg", 0, 76, 86.40m },
                    { 9, "Ketoprofen", 1, "Ketoprofen Jel, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/analgesics.png", false, 0, "Ketoprofen Jel", 0, 39, 132.20m },
                    { 10, "Kolekalsiferol", 2, "Vitamin D3 1000 IU, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/supplements.png", true, 1, "Vitamin D3 1000 IU", 0, 164, 214.90m },
                    { 11, "Magnezyum Sitrat", 2, "Magnezyum Complex, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/supplements.png", false, 1, "Magnezyum Complex", 0, 91, 319.50m },
                    { 12, "Lactobacillus Acidophilus", 2, "Probiyotik Balance, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/supplements.png", true, 1, "Probiyotik Balance", 0, 58, 384.00m },
                    { 13, "%10 Üre", 3, "Urea Repair Losyon, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/dermocosmetics.png", false, 1, "Urea Repair Losyon", 0, 72, 268.75m },
                    { 14, "Organik UV Filtreleri", 3, "Sun Protect SPF 50+, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/dermocosmetics.png", true, 1, "Sun Protect SPF 50+", 0, 105, 449.90m },
                    { 15, "Niasinamid", 3, "Niacinamide Serum, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/dermocosmetics.png", false, 1, "Niacinamide Serum", 0, 46, 398.50m },
                    { 16, "Medikal Cihaz", 4, "Dijital Ateş Ölçer, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2029, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/medical-supplies.png", true, 1, "Dijital Ateş Ölçer", 0, 130, 245.00m },
                    { 17, "Enzimatik Test Stribi", 4, "Glikoz Test Stribi 50'li, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2027, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/medical-supplies.png", false, 1, "Glikoz Test Stribi 50'li", 0, 88, 525.40m },
                    { 18, "Steril Pamuklu Gaz", 4, "Steril Gaz Kompres 100'lü, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.", new DateTime(2028, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "/images/products/medical-supplies.png", false, 1, "Steril Gaz Kompres 100'lü", 0, 210, 184.90m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=Paraset%20500%20mg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=%C4%B0bucold%20Plus");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=Omega%203%20Forte");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=Dermacare%20Krem");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=Diazepam%205%20mg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://placehold.co/640x520/f0f6ff/0047ab?text=Morfin%20Ampul");
        }
    }
}
