using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CrudPark_Back.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    identification_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "operators",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rate_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    hourly_rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    fraction_rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    daily_cap = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    grace_period_minutes = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    effective_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    license_plate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    vehicle_type = table.Column<string>(type: "text", nullable: false),
                    brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "monthly_subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    subscription_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    amount_paid = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    max_vehicles = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monthly_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_monthly_subscriptions_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shifts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    shift_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    shift_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    initial_cash = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    final_cash = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shifts", x => x.id);
                    table.ForeignKey(
                        name: "FK_shifts_operators_operator_id",
                        column: x => x.operator_id,
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "FK_customer_vehicles_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_vehicles_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscription_vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subscription_id = table.Column<int>(type: "integer", nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscription_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "FK_subscription_vehicles_monthly_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "monthly_subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_subscription_vehicles_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    folio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    subscription_id = table.Column<int>(type: "integer", nullable: true),
                    entry_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exit_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ticket_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    parking_duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    qr_code_data = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_monthly_subscriptions_subscription_id",
                        column: x => x.subscription_id,
                        principalTable: "monthly_subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tickets_operators_operator_id",
                        column: x => x.operator_id,
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_id = table.Column<int>(type: "integer", nullable: false),
                    operator_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    payment_method = table.Column<string>(type: "text", nullable: false),
                    payment_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_operators_operator_id",
                        column: x => x.operator_id,
                        principalTable: "operators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payments_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "created_at", "email", "full_name", "identification_number", "is_active", "phone", "updated_at" },
                values: new object[] { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "maria.gonzalez@email.com", "María González", "1234567890", true, "3001234567", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "operators",
                columns: new[] { "id", "created_at", "email", "full_name", "is_active", "password_hash", "updated_at", "username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "admin@crudpark.com", "Administrator", true, "$2a$11$ZpW5n3Z3Z3Z3Z3Z3Z3Z3Z.K5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J0J0", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "admin" },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "juan@crudpark.com", "Juan Pérez", true, "$2a$11$XpW5n3Z3Z3Z3Z3Z3Z3Z3Z.K5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J1", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "jperez" }
                });

            migrationBuilder.InsertData(
                table: "rates",
                columns: new[] { "id", "created_at", "daily_cap", "effective_from", "fraction_rate", "grace_period_minutes", "hourly_rate", "is_active", "rate_name", "updated_at" },
                values: new object[] { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 30000m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1000m, 30, 3000m, true, "Tarifa Estándar 2025", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "vehicles",
                columns: new[] { "id", "brand", "color", "created_at", "license_plate", "model", "updated_at", "vehicle_type" },
                values: new object[,]
                {
                    { 1, "Toyota", "Blanco", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "ABC123", "Corolla", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Car" },
                    { 2, "Yamaha", "Negro", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "XYZ789", "FZ", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Motorcycle" }
                });

            migrationBuilder.InsertData(
                table: "customer_vehicles",
                columns: new[] { "id", "created_at", "customer_id", "is_primary", "vehicle_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, true, 1 },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 2 }
                });

            migrationBuilder.InsertData(
                table: "monthly_subscriptions",
                columns: new[] { "id", "amount_paid", "created_at", "customer_id", "end_date", "is_active", "max_vehicles", "start_date", "subscription_code", "updated_at" },
                values: new object[] { 1, 150000m, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SUB-2025-001", new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "subscription_vehicles",
                columns: new[] { "id", "added_at", "subscription_id", "vehicle_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_vehicles_customer_id",
                table: "customer_vehicles",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_vehicles_vehicle_id",
                table: "customer_vehicles",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_email",
                table: "customers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_monthly_subscriptions_customer_id",
                table: "monthly_subscriptions",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_monthly_subscriptions_subscription_code",
                table: "monthly_subscriptions",
                column: "subscription_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_operators_username",
                table: "operators",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_operator_id",
                table: "payments",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ticket_id",
                table: "payments",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "IX_shifts_operator_id",
                table: "shifts",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_vehicles_subscription_id",
                table: "subscription_vehicles",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_subscription_vehicles_vehicle_id",
                table: "subscription_vehicles",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_folio",
                table: "tickets",
                column: "folio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_operator_id",
                table: "tickets",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_subscription_id",
                table: "tickets",
                column: "subscription_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_vehicle_id",
                table: "tickets",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_license_plate",
                table: "vehicles",
                column: "license_plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_vehicles");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "rates");

            migrationBuilder.DropTable(
                name: "shifts");

            migrationBuilder.DropTable(
                name: "subscription_vehicles");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "monthly_subscriptions");

            migrationBuilder.DropTable(
                name: "operators");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
