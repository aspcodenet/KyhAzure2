using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class AnsvarigMyndighet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnsvarigMyndighetId",
                table: "VaccineringsFaser",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Myndigheter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Myndigheter", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaccineringsFaser_AnsvarigMyndighetId",
                table: "VaccineringsFaser",
                column: "AnsvarigMyndighetId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineringsFaser_Myndigheter_AnsvarigMyndighetId",
                table: "VaccineringsFaser",
                column: "AnsvarigMyndighetId",
                principalTable: "Myndigheter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineringsFaser_Myndigheter_AnsvarigMyndighetId",
                table: "VaccineringsFaser");

            migrationBuilder.DropTable(
                name: "Myndigheter");

            migrationBuilder.DropIndex(
                name: "IX_VaccineringsFaser_AnsvarigMyndighetId",
                table: "VaccineringsFaser");

            migrationBuilder.DropColumn(
                name: "AnsvarigMyndighetId",
                table: "VaccineringsFaser");
        }
    }
}
