using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes;

namespace SkillUp.Tests.Features.Quizzes;

public class GetQuizByIdTest: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly SkillUpWebApplicationFactory _factory;
    
    public GetQuizByIdTest(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetQuizById_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var id = Guid.NewGuid();
        
        var response = await _client.GetAsync($"/api/quizzes/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetQuizById_ShouldReturnOkWithQuizDetails_WhenQuizExists()
    {
        var knownId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = knownId,
            Title = "Seeded"
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Quizzes.Add(existingQuiz);
            await db.SaveChangesAsync();
        }

        var response = await _client.GetAsync($"/api/quizzes/{knownId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetQuizById.QuizResponse>();
        result.Should().NotBeNull();
        result.Id.Should().Be(knownId);
        result.Title.Should().Be("Seeded");
    }
}