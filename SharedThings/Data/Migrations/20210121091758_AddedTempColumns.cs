using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class AddedTempColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AntalDoser",
                table: "Vacciner",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Vacciner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntalDoser",
                table: "Vacciner");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Vacciner");
        }
    }
}
