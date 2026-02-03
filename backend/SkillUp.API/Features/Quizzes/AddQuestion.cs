using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;
using SkillUp.API.Domain;
using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes;

public static class AddQuestion
{
    public record AnswerRequest(string Text, bool IsCorrect);
    public record QuestionRequest(QuestionType Type, string Text, List<AnswerRequest> Answers);
    
    public record AnswerResponse(Guid Id, string Text, bool IsCorrect);
    public record QuestionResponse(Guid Id, string Text, QuestionType Type, List<AnswerResponse>? Answers = null);
    
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/questions", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid id, AppDbContext context, QuestionRequest request)
    {
        var quiz = await context.Quizzes.FindAsync(id);

        if (quiz == null)
        {
            return Results.NotFound();
        }

        var question = new Question
        {
            Text = request.Text,
            Type = request.Type,
            QuizId = id,
            Answers = request.Answers.Select(a => new Answer
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList()
        };
        
        quiz.Questions.Add(question);
        await context.SaveChangesAsync();
        
        var questionResponse = new QuestionResponse
        (
            question.Id, 
            question.Text, 
            question.Type, 
            question.Answers.Select(a => new AnswerResponse(a.Id, a.Text, a.IsCorrect)).ToList()
        );
        
        return Results.Created($"/api/quizzes/{id}/questions/{question.Id}", questionResponse);
    }
}