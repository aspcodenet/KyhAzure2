using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class VaccinationsFaser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VaccineringsFasId",
                table: "Personer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VaccineringsFaser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineringsFaser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personer_VaccineringsFasId",
                table: "Personer",
                column: "VaccineringsFasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personer_VaccineringsFaser_VaccineringsFasId",
                table: "Personer",
                column: "VaccineringsFasId",
                principalTable: "VaccineringsFaser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personer_VaccineringsFaser_VaccineringsFasId",
                table: "Personer");

            migrationBuilder.DropTable(
                name: "VaccineringsFaser");

            migrationBuilder.DropIndex(
                name: "IX_Personer_VaccineringsFasId",
                table: "Personer");

            migrationBuilder.DropColumn(
                name: "VaccineringsFasId",
                table: "Personer");
        }
    }
}
