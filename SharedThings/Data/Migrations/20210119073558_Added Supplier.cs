using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc1VaccinDemo.Data.Migrations
{
    public partial class AddedSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Vacciner");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Vacciner",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacciner_SupplierId",
                table: "Vacciner",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacciner_Suppliers_SupplierId",
                table: "Vacciner",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacciner_Suppliers_SupplierId",
                table: "Vacciner");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Vacciner_SupplierId",
                table: "Vacciner");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Vacciner");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Vacciner",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
