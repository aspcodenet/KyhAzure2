using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class Personfixes4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PreliminaryNextVaccinDate",
                table: "Personer",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PreliminaryNextVaccinDate",
                table: "Personer",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
