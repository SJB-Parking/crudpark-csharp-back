using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudPark_Back.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOperatorPasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "operators",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "$2a$11$kH3ulu5AEQGioyzXDx.pg.4JDqE9/mACqZbtdymRdAm.zgUN2rX7.");

            migrationBuilder.UpdateData(
                table: "operators",
                keyColumn: "id",
                keyValue: 2,
                column: "password_hash",
                value: "$2a$11$a2ISYKBQqXw.27Xd8WwykOc9YpYSZCfKVvV/WqdEZ7c3mdAY4d88K");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "operators",
                keyColumn: "id",
                keyValue: 1,
                column: "password_hash",
                value: "$2a$11$ZpW5n3Z3Z3Z3Z3Z3Z3Z3Z.K5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J0J0");

            migrationBuilder.UpdateData(
                table: "operators",
                keyColumn: "id",
                keyValue: 2,
                column: "password_hash",
                value: "$2a$11$XpW5n3Z3Z3Z3Z3Z3Z3Z3Z.K5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J1");
        }
    }
}
