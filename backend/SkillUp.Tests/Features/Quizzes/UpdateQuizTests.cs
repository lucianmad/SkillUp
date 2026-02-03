using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes;

public class UpdateQuizTests: BaseIntegrationTest
{
    public UpdateQuizTests(SkillUpWebApplicationFactory factory) : base(factory) {}  

    [Fact]
    public async Task PutQuiz_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var id = Guid.NewGuid();
        var request = new QuizRequest("Integration testing");

        var response = await Client.PutAsJsonAsync($"/api/quizzes/{id}", request);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task PutQuiz_ShouldReturnNoContent_WhenUpdatingAValidQuiz()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Seeded"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var request = new UpdateQuiz.QuizRequest("Integration testing");
        
        var response = await Client.PutAsJsonAsync($"/api/quizzes/{existingId}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}