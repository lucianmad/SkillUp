using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillUp.API.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexForQuizTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Title",
                table: "Quizzes",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_Title",
                table: "Quizzes");
        }
    }
}
