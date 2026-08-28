using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmaSupply.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdditionalPharmacies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pharmacies",
                columns: new[] { "Id", "Balance", "CreditLimit", "IsLicenseActive", "LicenseNumber", "MonthlyRedPrescriptionQuota", "Name", "UsedRedPrescriptionQuota" },
                values: new object[,]
                {
                    { 2, 18500m, 60000m, true, "ECZ-34002", 80, "Hayat Eczanesi", 0 },
                    { 3, 22000m, 90000m, true, "ECZ-34003", 120, "Mavi Eczanesi", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pharmacies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pharmacies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
