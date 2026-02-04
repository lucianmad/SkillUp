using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Features.Quizzes.GetQuizById;

namespace SkillUp.Tests.Features.Quizzes.GetQuizById;

public class GetQuizByIdEndpointTests: BaseIntegrationTest
{
    
    public GetQuizByIdEndpointTests(SkillUpWebApplicationFactory factory) : base(factory) {}
    
    [Fact]
    public async Task GetQuizById_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var id = Guid.NewGuid();
        
        var response = await Client.GetAsync($"/api/quizzes/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetQuizById_ShouldReturnOkWithQuizDetails_WhenQuizExists()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Seeded"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
            
        var response = await Client.GetAsync($"/api/quizzes/{existingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<QuizResponse>();
        result.Should().NotBeNull();
        result.Id.Should().Be(existingId);
        result.Title.Should().Be("Seeded");
    }
}