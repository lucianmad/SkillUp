using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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
    public async Task PostQuiz_ShouldReturnCreatedQuizWithAGuidAndATitle_WhenCreatingAValidQuiz()
    {
        var request = new PostQuiz.QuizRequest("Integration testing");
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<PostQuiz.QuizResponse>();
        result.Should().NotBeNull();
        result.Title.Should().Be("Integration testing");
        result.Id.Should().NotBeEmpty();
    }
}