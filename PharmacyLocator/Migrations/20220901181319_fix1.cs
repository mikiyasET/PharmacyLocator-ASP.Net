using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyLocator.Migrations
{
    public partial class fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pharmacies_Username",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Lng",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "pharmacies");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "pharmacies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pharmacies_Email",
                table: "pharmacies",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pharmacies_Email",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "pharmacies");

            migrationBuilder.AddColumn<int>(
                name: "Lat",
                table: "pharmacies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lng",
                table: "pharmacies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "pharmacies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_pharmacies_Username",
                table: "pharmacies",
                column: "Username",
                unique: true);
        }
    }
}
