using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CrudPark_Back.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleTypeToRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "vehicle_type",
                table: "rates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "daily_cap", "fraction_rate", "hourly_rate", "rate_name", "vehicle_type" },
                values: new object[] { 20000m, 700m, 2000m, "Tarifa Motos 2025", "Motorcycle" });

            migrationBuilder.InsertData(
                table: "rates",
                columns: new[] { "id", "created_at", "daily_cap", "effective_from", "fraction_rate", "grace_period_minutes", "hourly_rate", "is_active", "rate_name", "updated_at", "vehicle_type" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 30000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1000m, 30, 3000m, true, "Tarifa Carros 2025", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Car" },
                    { 3, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 50000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1500m, 30, 5000m, true, "Tarifa Camiones 2025", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Truck" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "rates",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "vehicle_type",
                table: "rates");

            migrationBuilder.UpdateData(
                table: "rates",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "daily_cap", "fraction_rate", "hourly_rate", "rate_name" },
                values: new object[] { 30000m, 1000m, 3000m, "Tarifa Estándar 2025" });
        }
    }
}
