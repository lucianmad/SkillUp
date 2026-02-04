using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.UpdateQuestion;

namespace SkillUp.Tests.Features.Quizzes.UpdateQuestion;

public class UpdateQuestionEndpointTests: BaseIntegrationTest
{
    public UpdateQuestionEndpointTests(SkillUpWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task UpdateQuestion_ShouldReturnNoContent_WhenUpdatingAValidQuestion()
    {
        var existingQuizId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingQuizId,
            Title = "Test quiz"
        };
        
        var existingQuestionId = Guid.NewGuid();
        var existingQuestion = new Question
        {
            Id = existingQuestionId,
            Text = "Test question",
            Type = QuestionType.MultipleChoice,
            Answers =
            [
                new Answer { Text = "A", IsCorrect = true },
                new Answer { Text = "B", IsCorrect = false },
                new Answer { Text = "C", IsCorrect = true },
                new Answer { Text = "D", IsCorrect = false }
            ]
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        existingQuiz.Questions.Add(existingQuestion);
        await DbContext.SaveChangesAsync();

        var firstAnswer = new UpdateAnswerRequest("A", false);
        var secondAnswer = new UpdateAnswerRequest("B", true);
        var answers = new List<UpdateAnswerRequest> {firstAnswer, secondAnswer};
        var request = new UpdateQuestionRequest(QuestionType.SingleChoice, "What is the response", answers);
        
        var response = await Client.PutAsJsonAsync($"/api/quizzes/{existingQuizId}/questions/{existingQuestionId}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateQuestion_ShouldReturnNotFound_WhenUpdatingAQuestionThatDoesNotExist()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Test quiz"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var firstAnswer = new UpdateAnswerRequest("A", false);
        var secondAnswer = new UpdateAnswerRequest("B", true);
        var answers = new List<UpdateAnswerRequest> {firstAnswer, secondAnswer};
        var request = new UpdateQuestionRequest(QuestionType.SingleChoice, "What is the response", answers);
        
        var response = await Client.PutAsJsonAsync($"/api/quizzes/{existingId}/questions/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UpdateQuestion_ShouldReturnNotFound_WhenUpdatingAQuestionOfNonExistingQuiz()
    {
        var firstAnswer = new UpdateAnswerRequest("A", false);
        var secondAnswer = new UpdateAnswerRequest("B", true);
        var answers = new List<UpdateAnswerRequest> {firstAnswer, secondAnswer};
        var request = new UpdateQuestionRequest(QuestionType.SingleChoice, "What is the response", answers);
        
        var response = await Client.PutAsJsonAsync($"/api/quizzes/{Guid.NewGuid()}/questions/{Guid.NewGuid()}", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);   
    }
}