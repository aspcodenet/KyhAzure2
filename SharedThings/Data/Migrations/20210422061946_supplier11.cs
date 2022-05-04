using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class supplier11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UnderInvestigation",
                table: "Suppliers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UnderInvestigation",
                table: "Suppliers");
        }
    }
}
