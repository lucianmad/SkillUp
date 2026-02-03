using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes.CreateQuiz;

public class CreateQuizEndpointTests: BaseIntegrationTest
{
    public CreateQuizEndpointTests(SkillUpWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task PostQuiz_ShouldReturnCreatedQuizWithAGuidAndATitle_WhenCreatingAValidQuiz()
    {
        var request = new QuizRequest("Integration testing");
        
        var response = await Client.PostAsJsonAsync("api/quizzes", request);
        
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
        
        var response = await Client.PostAsJsonAsync("api/quizzes", request);
        
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

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var request = new QuizRequest("Seeded");
        
        var response = await Client.PostAsJsonAsync("api/quizzes", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}