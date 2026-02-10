using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.GetQuestions;

namespace SkillUp.Tests.Features.Quizzes.GetQuestions;

public class GetQuestionsEndpointTests: BaseIntegrationTest
{
    public GetQuestionsEndpointTests(SkillUpWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetQuestions_ShouldReturnOk_WhenNoQuestionsExist()
    {
        var existingId = Guid.NewGuid();
        var existingQuiz = new Quiz
        {
            Id = existingId,
            Title = "Test quiz"
        };

        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.GetAsync($"/api/quizzes/{existingId}/questions");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<QuestionResponse>>();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetQuestions_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var response = await Client.GetAsync("/api/quizzes/00000000-0000-0000-0000-000000000000/questions");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetQuestions_ShouldReturnOkWithQuestionList_WhenQuestionsExist()
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
            Type = QuestionType.SingleChoice
        };
        
        existingQuiz.Questions.Add(existingQuestion);
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var response = await Client.GetAsync($"/api/quizzes/{existingQuizId}/questions");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<QuestionResponse>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}