using System.Net;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;

namespace SkillUp.Tests.Features.Quizzes.DeleteQuestion;

public class DeleteQuestionEndpointTests: BaseIntegrationTest
{
    public DeleteQuestionEndpointTests(SkillUpWebApplicationFactory factory) : base(factory) {}
    
    [Fact]
    public async Task DeleteQuestion_ShouldReturnNoContent_WhenDeletingAValidQuestion()
    {
        var existingQuizId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingQuizId,
            Title = "Test quiz"
        };

        var firstAnswer = new Answer { Text = "A", IsCorrect = true };
        var secondAnswer = new Answer { Text = "B", IsCorrect = false };
        var thirdAnswer = new Answer { Text = "C", IsCorrect = false };
        
        var existingQuestionId = Guid.NewGuid();
        var existingQuestion = new Question
        {
            Id = existingQuestionId,
            Text = "Test question",
            Type = QuestionType.SingleChoice,
            Answers = [firstAnswer, secondAnswer, thirdAnswer]
        };
        
        existingQuiz.Questions.Add(existingQuestion);
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.DeleteAsync($"/api/quizzes/{existingQuizId}/questions/{existingQuestionId}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DbContext.Questions.Should().BeEmpty();
    }
    
    [Fact]
    public async Task DeleteQuestion_ShouldReturnNotFound_WhenDeletingNonExistingQuestion()
    {
        var existingQuizId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingQuizId,
            Title = "Test quiz"
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.DeleteAsync($"/api/quizzes/{existingQuizId}/questions/{Guid.NewGuid()}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task DeleteQuestion_ShouldReturnNotFound_WhenDeletingQuestionOfNonExistingQuiz()
    {
        var response = await Client.DeleteAsync($"/api/quizzes/{Guid.NewGuid()}/questions/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}