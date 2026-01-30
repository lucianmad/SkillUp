using System.Net;
using System.Net.Http.Json;
using SkillUp.API.Features.Quizzes;

namespace SkillUp.Tests.Features.Quizzes;

public class PostQuizTest: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PostQuizTest(SkillUpWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostQuiz_ReturnsCreatedQuiz_WhenCreatingAValidQuiz()
    {
        var request = new PostQuiz.QuizRequest("Integration testing");
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        var result = await response.Content.ReadFromJsonAsync<PostQuiz.QuizResponse>();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal("Integration testing", result.Title);
        Assert.NotEqual(Guid.Empty, result.Id);
    }
}