using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfoedukaApi.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Kolegiji",
                columns: new[] { "Id", "Naziv", "Opis" },
                values: new object[,]
                {
                    { 1, "Projektni Razvoj Aplikacija", "Kolegij o razvoju aplikacija" },
                    { 2, "Baze Podataka", "Kolegij o bazama podataka" }
                });

            migrationBuilder.UpdateData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 1,
                column: "Lozinka",
                value: "$2a$11$1pyL3mPQImM3.nF5m9eyl.buUnI4g65QO/ySfbNRMQ9E6r2XpfkKe");

            migrationBuilder.InsertData(
                table: "Korisnici",
                columns: new[] { "Id", "Email", "Ime", "Lozinka", "Prezime", "Tip" },
                values: new object[] { 2, "ivan@infoeduka.hr", "Ivan", "$2a$11$oYERp7IfqIS3gb/bti4uqei8zeaNm/KQcZ3FCciuYxuzldwGNoftK", "Ivić", "Predavac" });

            migrationBuilder.InsertData(
                table: "KolegijPredavac",
                columns: new[] { "KolegijiId", "PredavaciId" },
                values: new object[] { 1, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KolegijPredavac",
                keyColumns: new[] { "KolegijiId", "PredavaciId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Kolegiji",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Kolegiji",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 1,
                column: "Lozinka",
                value: "$2a$11$F4lxURzCnzyJZbn2siTeBuk74km890i9N3CF./CC4f1KQ6C0eE0au");
        }
    }
}
