using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes;

public class GetQuizzesTests : BaseIntegrationTest
{
    public GetQuizzesTests(SkillUpWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task GetQuizzes_ShouldReturnOkAndEmptyList_WhenNoQuizzesExist()
    {
        var response = await Client.GetAsync("/api/quizzes");
        
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

        DbContext.Quizzes.Add(firstQuiz);
        DbContext.Quizzes.Add(secondQuiz);
        DbContext.Quizzes.Add(thirdQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.GetAsync("/api/quizzes");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<QuizResponse>>();
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }
}