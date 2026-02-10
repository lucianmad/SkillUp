using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.SubmitAnswers;

namespace SkillUp.Tests.Features.Quizzes.SubmitAnswers;

public class SubmitAnswersEndpointTests: BaseIntegrationTest
{
    public SubmitAnswersEndpointTests(SkillUpWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task SubmitAnswers_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        var response =
            await Client.PostAsJsonAsync($"/api/quizzes/{Guid.NewGuid()}/submit", new SubmitAnswersRequest([]));
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task SubmitAnswers_ShouldReturnOkAndDoScoreCalculationCorrectly_WhenHavingMixedQuestions()
    {
        var quizId = Guid.NewGuid();
        var question1Id = Guid.NewGuid();
        var question2Id = Guid.NewGuid();
        
        var question1A = new Answer { Id = Guid.NewGuid(), Text = "Correct answer", IsCorrect = true };
        var question1B = new Answer { Id = Guid.NewGuid(), Text = "Wrong answer", IsCorrect = false };
        
        var question2A = new Answer { Id = Guid.NewGuid(), Text = "Wrong answer", IsCorrect = false };
        var question2B = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 1", IsCorrect = true };
        var question2C = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 2", IsCorrect = true };

        var existingQuiz = new Quiz
        {
            Id = quizId,
            Title = "Test quiz",
            Questions =
            [
                new Question
                {
                    Id = question1Id, Text = "Q1", Type = QuestionType.SingleChoice,
                    Answers = [question1A, question1B]
                },
                new Question
                {
                    Id = question2Id, Text = "Q2", Type = QuestionType.MultipleChoice,
                    Answers = [question2A, question2B, question2C]
                }
            ]
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();

        var request = new SubmitAnswersRequest([
            new SubmitAnswerDTO(question1Id, [question1A.Id]),
            new SubmitAnswerDTO(question2Id, [question2B.Id, question2C.Id])
        ]);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{quizId}/submit", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SubmitAnswersResponse>();
        result.Should().NotBeNull();
        result.Score.Should().Be(2);
        result.TotalQuestions.Should().Be(2);
    }

    [Fact]
    public async Task SubmitAnswers_ShouldReturnOkWithScoreZero_WhenMultiChoiceIsPartiallyCorrect()
    {
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        
        var questionA = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 1", IsCorrect = true };
        var questionB = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 2", IsCorrect = true };

        var existingQuiz = new Quiz
        {
            Id = quizId,
            Title = "Test quiz",
            Questions =
            [
                new Question
                {
                    Id = questionId, Text = "Q1", Type = QuestionType.MultipleChoice, Answers = [questionA, questionB]
                }
            ]
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var request = new SubmitAnswersRequest([new SubmitAnswerDTO(questionId, [questionA.Id])]);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{quizId}/submit", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SubmitAnswersResponse>();
        result.Should().NotBeNull();
        result.Score.Should().Be(0);
        result.TotalQuestions.Should().Be(1);
    }

    [Fact]
    public async Task SubmitAnswers_ShouldReturnOkWithScoreZero_WhenUserSelectsExtraWrongOption()
    {
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        
        var questionA = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 1", IsCorrect = true };
        var questionB = new Answer { Id = Guid.NewGuid(), Text = "Correct answer 2", IsCorrect = true };
        var questionC = new Answer { Id = Guid.NewGuid(), Text = "Wrong answer", IsCorrect = false };

        var existingQuiz = new Quiz
        {
            Id = quizId,
            Title = "Test quiz",
            Questions =
            [
                new Question
                {
                    Id = questionId, Text = "Q1", Type = QuestionType.MultipleChoice,
                    Answers = [questionA, questionB, questionC]
                }
            ]
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();
        
        var request = new SubmitAnswersRequest([new SubmitAnswerDTO(questionId, [questionA.Id, questionC.Id])]);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{quizId}/submit", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SubmitAnswersResponse>();
        result.Should().NotBeNull();
        result.Score.Should().Be(0);
        result.TotalQuestions.Should().Be(1);
    }

    [Fact]
    public async Task SubmitAnswers_ShouldReturnOkAndIgnoreDuplicates_WhenUserSendsTheSameQuestionsAndAnswersToPreventCheating()
    {
        var quizId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        
        var questionA = new Answer { Id = Guid.NewGuid(), Text = "Correct answer", IsCorrect = true };
        var questionB = new Answer { Id = Guid.NewGuid(), Text = "Wrong answer", IsCorrect = false };

        var existingQuiz = new Quiz
        {
            Id = quizId,
            Title = "Test quiz",
            Questions =
            [
                new Question
                {
                    Id = questionId, Text = "Q1", Type = QuestionType.SingleChoice,
                    Answers = [questionA, questionB]
                }
            ]
        };
        
        DbContext.Quizzes.Add(existingQuiz);
        await DbContext.SaveChangesAsync();

        var request = new SubmitAnswersRequest(
            [
                new SubmitAnswerDTO(questionId, [questionA.Id]),
                new SubmitAnswerDTO(questionId, [questionA.Id]),
                new SubmitAnswerDTO(questionId, [questionA.Id])
            ]);
        
        var response = await Client.PostAsJsonAsync($"/api/quizzes/{quizId}/submit", request);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SubmitAnswersResponse>();
        result.Should().NotBeNull();
        result.Score.Should().Be(1);
        result.TotalQuestions.Should().Be(1);
    }
}