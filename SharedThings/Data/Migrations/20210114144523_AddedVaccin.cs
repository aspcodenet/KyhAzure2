using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class AddedVaccin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacciner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Supplier = table.Column<string>(maxLength: 100, nullable: true),
                    Namn = table.Column<string>(maxLength: 50, nullable: true),
                    EuOkStatus = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacciner", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacciner");
        }
    }
}
