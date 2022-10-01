using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyLocator.Migrations
{
    public partial class requestAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pharmacies_admins_AddBy",
                table: "pharmacies");

            migrationBuilder.AlterColumn<long>(
                name: "AddBy",
                table: "pharmacies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_requests_Name",
                table: "requests",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_pharmacies_admins_AddBy",
                table: "pharmacies",
                column: "AddBy",
                principalTable: "admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pharmacies_admins_AddBy",
                table: "pharmacies");

            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.AlterColumn<long>(
                name: "AddBy",
                table: "pharmacies",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_pharmacies_admins_AddBy",
                table: "pharmacies",
                column: "AddBy",
                principalTable: "admins",
                principalColumn: "Id");
        }
    }
}
