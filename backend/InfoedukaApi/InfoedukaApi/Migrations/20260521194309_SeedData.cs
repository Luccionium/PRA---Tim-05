using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoedukaApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 1,
                column: "Lozinka",
                value: "$2a$11$F4lxURzCnzyJZbn2siTeBuk74km890i9N3CF./CC4f1KQ6C0eE0au");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 1,
                column: "Lozinka",
                value: "$2a$11$PttD8PK/rViiotbTyPt0OOSsWO4kMS11R4jiWH3iMVWlThTFOK/IS");
        }
    }
}
