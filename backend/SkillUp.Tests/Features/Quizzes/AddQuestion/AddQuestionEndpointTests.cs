using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.AddQuestion;

namespace SkillUp.Tests.Features.Quizzes.AddQuestion;

public class AddQuestionEndpointTests: BaseIntegrationTest
{
    public AddQuestionEndpointTests(SkillUpWebApplicationFactory factory) : base(factory) {}   
    
    [Fact]
    public async Task AddQuestion_ShouldReturnCreatedQuestionWithDetails_WhenAddingAValidQuestion()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = $"Test Quiz {Guid.NewGuid()}"
        };

            DbContext.Quizzes.Add(existingQuiz);
            await DbContext.SaveChangesAsync();

        var firstAnswer = new AnswerRequest("A", false);
        var secondAnswer = new AnswerRequest("B", true);
        var thirdAnswer = new AnswerRequest("C", false);
        var answers = new List<AnswerRequest> {firstAnswer, secondAnswer, thirdAnswer};
        var request = new QuestionRequest(QuestionType.SingleChoice, "What is the response", answers);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{existingId}/questions", request);

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
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{id}/questions", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task AddQuestion_ShouldReturnBadRequest_WhenAddingAQuestionWithEmptyText()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = $"Test Quiz {Guid.NewGuid()}"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();

        var request = new QuestionRequest(QuestionType.SingleChoice, string.Empty, []);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{existingId}/questions", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}