using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes;

public class UpdateQuizTests: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly SkillUpWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UpdateQuizTests(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PutQuiz_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var id = Guid.NewGuid();
        var request = new QuizRequest("Integration testing");

        var response = await _client.PutAsJsonAsync($"/api/quizzes/{id}", request);
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

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Quizzes.Add(existingQuiz);
            await db.SaveChangesAsync();
        }
        
        var request = new UpdateQuiz.QuizRequest("Integration testing");
        
        var response = await _client.PutAsJsonAsync($"/api/quizzes/{existingId}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}