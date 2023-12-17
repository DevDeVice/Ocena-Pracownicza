using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ocena_Pracownicza.Migrations
{
    /// <inheritdoc />
    public partial class RenameEvaluationsToEvaluationBiuro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluations",
                table: "Evaluations");

            migrationBuilder.RenameTable(
                name: "Evaluations",
                newName: "EvaluationBiuro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EvaluationBiuro",
                table: "EvaluationBiuro",
                column: "EvaluationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EvaluationBiuro",
                table: "EvaluationBiuro");

            migrationBuilder.RenameTable(
                name: "EvaluationBiuro",
                newName: "Evaluations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluations",
                table: "Evaluations",
                column: "EvaluationID");
        }
    }
}
