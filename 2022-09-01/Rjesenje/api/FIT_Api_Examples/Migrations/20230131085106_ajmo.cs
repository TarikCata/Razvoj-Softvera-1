using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_Api_Examples.Migrations
{
    public partial class ajmo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "evidentiraoKorisnikId",
                table: "Godina",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Godina_evidentiraoKorisnikId",
                table: "Godina",
                column: "evidentiraoKorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Godina_KorisnickiNalog_evidentiraoKorisnikId",
                table: "Godina",
                column: "evidentiraoKorisnikId",
                principalTable: "KorisnickiNalog",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Godina_KorisnickiNalog_evidentiraoKorisnikId",
                table: "Godina");

            migrationBuilder.DropIndex(
                name: "IX_Godina_evidentiraoKorisnikId",
                table: "Godina");

            migrationBuilder.DropColumn(
                name: "evidentiraoKorisnikId",
                table: "Godina");
        }
    }
}
