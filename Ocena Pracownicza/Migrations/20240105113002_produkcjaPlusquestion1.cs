using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ocena_Pracownicza.Migrations
{
    /// <inheritdoc />
    public partial class produkcjaPlusquestion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Question5",
                table: "EvaluationProdukcjaAnswers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question5",
                table: "EvaluationProdukcjaAnswers");
        }
    }
}
