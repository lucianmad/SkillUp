using System.Net;
using FluentAssertions;
using SkillUp.API.Domain;

namespace SkillUp.Tests.Features.Quizzes;

public class DeleteQuizTests: BaseIntegrationTest
{
    public DeleteQuizTests(SkillUpWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task DeleteQuiz_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var id = Guid.NewGuid();

        var response = await Client.DeleteAsync($"/api/quizzes/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteQuiz_ShouldReturnNoContent_WhenQuizExists()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Seeded"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.DeleteAsync($"/api/quizzes/{existingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}