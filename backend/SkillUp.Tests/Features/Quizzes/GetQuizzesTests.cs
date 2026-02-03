using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes;

public class GetQuizzesTests : IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly SkillUpWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GetQuizzesTests(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetQuizzes_ShouldReturnOkAndEmptyList_WhenNoQuizzesExist()
    {
        var response = await _client.GetAsync("/api/quizzes");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<QuizResponse>>();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetQuizzes_ShouldReturnOkAndListOfQuizzes_WhenQuizzesExist()
    {
        var firstQuizId = Guid.NewGuid();
        var secondQuizId = Guid.NewGuid();
        var thirdQuizId = Guid.NewGuid();
        var firstQuiz = new Quiz
        {
            Id = firstQuizId,
            Title = "First quiz"
        };
        var secondQuiz = new Quiz
        {
            Id = secondQuizId,
            Title = "Second quiz"
        };
        var thirdQuiz = new Quiz
        {
            Id = thirdQuizId,
            Title = "Third quiz"
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Quizzes.Add(firstQuiz);
            db.Quizzes.Add(secondQuiz);
            db.Quizzes.Add(thirdQuiz);
            await db.SaveChangesAsync();
        }
        
        var response = await _client.GetAsync("/api/quizzes");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<QuizResponse>>();
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
}