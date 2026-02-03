using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Features.Quizzes;

namespace SkillUp.Tests.Features.Quizzes;

public class GetQuizzesTest : IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly HttpClient _client;

    public GetQuizzesTest(SkillUpWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetQuizzes_ShouldReturnOkAndEmptyList_WhenNoQuizzesExist()
    {
        var response = await _client.GetAsync("/api/quizzes");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<CreateQuiz.QuizResponse>>();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}