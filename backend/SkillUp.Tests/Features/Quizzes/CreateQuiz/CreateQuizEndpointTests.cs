using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes.CreateQuiz;

public class CreateQuizEndpointTests: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly SkillUpWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CreateQuizEndpointTests(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostQuiz_ShouldReturnCreatedQuizWithAGuidAndATitle_WhenCreatingAValidQuiz()
    {
        var request = new QuizRequest("Integration testing");
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<QuizResponse>();
        result.Should().NotBeNull();
        result.Title.Should().Be("Integration testing");
        result.Id.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task PostQuiz_ShouldReturnBadRequest_WhenCreatingAQuizWithEmptyTitle()
    {
        var request = new QuizRequest(string.Empty);
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostQuiz_ShouldReturnConflict_WhenCreatingAQuizWithAnExistingTitle()
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
        
        var request = new QuizRequest("Seeded");
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}