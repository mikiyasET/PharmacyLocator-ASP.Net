using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyLocator.Migrations
{
    public partial class locationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "pharmacies");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "pharmacies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "pharmacies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "pharmacies");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "pharmacies");

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "pharmacies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
