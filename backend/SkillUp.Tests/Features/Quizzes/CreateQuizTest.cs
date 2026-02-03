using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Features.Quizzes;

namespace SkillUp.Tests.Features.Quizzes;

public class CreateQuizTest: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateQuizTest(SkillUpWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostQuiz_ShouldReturnCreatedQuizWithAGuidAndATitle_WhenCreatingAValidQuiz()
    {
        var request = new CreateQuiz.QuizRequest("Integration testing");
        
        var response = await _client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateQuiz.QuizResponse>();
        result.Should().NotBeNull();
        result.Title.Should().Be("Integration testing");
        result.Id.Should().NotBeEmpty();
    }
}