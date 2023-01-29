using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_Api_Examples.Migrations
{
    public partial class addStudentOnGodina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "UpisanaGodina",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UpisanaGodina_StudentId",
                table: "UpisanaGodina",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UpisanaGodina_Student_StudentId",
                table: "UpisanaGodina",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpisanaGodina_Student_StudentId",
                table: "UpisanaGodina");

            migrationBuilder.DropIndex(
                name: "IX_UpisanaGodina_StudentId",
                table: "UpisanaGodina");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "UpisanaGodina");
        }
    }
}
