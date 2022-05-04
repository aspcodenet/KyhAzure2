using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class Personfixes3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedInSystem",
                table: "Personer");

            migrationBuilder.AddColumn<DateTime>(
                name: "PreliminaryNextVaccinDate",
                table: "Personer",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreliminaryNextVaccinDate",
                table: "Personer");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedInSystem",
                table: "Personer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
