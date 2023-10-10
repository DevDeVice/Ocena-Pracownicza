using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ocena_Pracownicza.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEvaluationToINT2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_Users_UserID",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_UserID",
                table: "Evaluations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_UserID",
                table: "Evaluations",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_Users_UserID",
                table: "Evaluations",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
