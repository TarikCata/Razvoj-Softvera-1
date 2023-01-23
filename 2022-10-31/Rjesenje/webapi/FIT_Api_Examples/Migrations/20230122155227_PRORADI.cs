using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_Api_Examples.Migrations
{
    public partial class PRORADI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpisanaGodina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GodinaStudija = table.Column<int>(type: "int", nullable: false),
                    AkademskaId = table.Column<int>(type: "int", nullable: true),
                    AkademskaGodinaid = table.Column<int>(type: "int", nullable: true),
                    CijenaSkolarine = table.Column<float>(type: "real", nullable: false),
                    Obnova = table.Column<bool>(type: "bit", nullable: false),
                    DatumOvjere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NapomenaZaOvjeru = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpisanaGodina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpisanaGodina_AkademskaGodina_AkademskaGodinaid",
                        column: x => x.AkademskaGodinaid,
                        principalTable: "AkademskaGodina",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpisanaGodina_AkademskaGodinaid",
                table: "UpisanaGodina",
                column: "AkademskaGodinaid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpisanaGodina");
        }
    }
}
