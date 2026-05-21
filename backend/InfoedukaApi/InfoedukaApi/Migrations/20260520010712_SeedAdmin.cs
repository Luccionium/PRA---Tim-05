using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfoedukaApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Korisnici",
                columns: new[] { "Id", "Email", "Ime", "Lozinka", "Prezime", "Tip" },
                values: new object[] { 1, "admin@infoeduka.hr", "Admin", "$2a$11$PttD8PK/rViiotbTyPt0OOSsWO4kMS11R4jiWH3iMVWlThTFOK/IS", "Admin", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Korisnici",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
