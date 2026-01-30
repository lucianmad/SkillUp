using System.Net.Http.Json;
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
    public async Task GetQuizzes_ReturnsOkAndEmptyList_WhenNoQuizzesExist()
    {
        var response = await _client.GetAsync("/api/quizzes");
        
        var result = await response.Content.ReadFromJsonAsync<List<PostQuiz.QuizResponse>>();
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}