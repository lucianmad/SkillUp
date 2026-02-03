using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.AddQuestion;

namespace SkillUp.Tests.Features.Quizzes.AddQuestion;

public class AddQuestionEndpointTests: IClassFixture<SkillUpWebApplicationFactory>
{
    private readonly SkillUpWebApplicationFactory _factory;
    private readonly HttpClient _client;
    
    public AddQuestionEndpointTests(SkillUpWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task AddQuestion_ShouldReturnCreatedQuestionWithDetails_WhenAddingAValidQuestion()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Seeded"
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Quizzes.Add(existingQuiz);
            await db.SaveChangesAsync();
        }

        var firstAnswer = new AnswerRequest("A", false);
        var secondAnswer = new AnswerRequest("B", true);
        var thirdAnswer = new AnswerRequest("C", false);
        var answers = new List<AnswerRequest> {firstAnswer, secondAnswer, thirdAnswer};
        var request = new QuestionRequest(QuestionType.SingleChoice, "What is the response", answers);
        
        var response = await _client.PostAsJsonAsync($"/api/quizzes/{existingId}/questions", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<QuestionResponse>();
        result.Should().NotBeNull();
        result.Text.Should().Be("What is the response");
        result.Type.Should().Be(QuestionType.SingleChoice);
        result.Answers.Should().HaveCount(3);
    }

    [Fact]
    public async Task AddQuestion_ShouldReturnNotFound_WhenAddingAQuestionToNonExistingQuiz()
    {
        var id = Guid.NewGuid();
        var firstAnswer = new AnswerRequest("A", false);
        var secondAnswer = new AnswerRequest("B", true);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [firstAnswer, secondAnswer]);
        
        var response = await _client.PostAsJsonAsync($"/api/quizzes/{id}/questions", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task AddQuestion_ShouldReturnBadRequest_WhenAddingAQuestionWithEmptyText()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Seeded"
        };

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Quizzes.Add(existingQuiz);
            await db.SaveChangesAsync();
        }

        var request = new QuestionRequest(QuestionType.SingleChoice, string.Empty, []);
        
        var response = await _client.PostAsJsonAsync($"/api/quizzes/{existingId}/questions", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}