using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_Api_Examples.Migrations
{
    public partial class addGodina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Godina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GodinaStudija = table.Column<int>(type: "int", nullable: false),
                    Cijena = table.Column<float>(type: "real", nullable: false),
                    Obnova = table.Column<bool>(type: "bit", nullable: false),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumOvjere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    akademskaId = table.Column<int>(type: "int", nullable: false),
                    AkademskaGodinaid = table.Column<int>(type: "int", nullable: true),
                    studentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Godina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Godina_AkademskaGodina_AkademskaGodinaid",
                        column: x => x.AkademskaGodinaid,
                        principalTable: "AkademskaGodina",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Godina_Student_studentId",
                        column: x => x.studentId,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Godina_AkademskaGodinaid",
                table: "Godina",
                column: "AkademskaGodinaid");

            migrationBuilder.CreateIndex(
                name: "IX_Godina_studentId",
                table: "Godina",
                column: "studentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Godina");
        }
    }
}
