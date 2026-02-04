using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;
using SkillUp.API.Domain;

namespace SkillUp.API.Features.Quizzes.AddQuestion;

public static class AddQuestionEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/questions", HandleAsync);
    }

    private static async Task<Results<Created<CreateQuestionResponse>, NotFound, ValidationProblem>> HandleAsync(
            Guid id, 
            AppDbContext context, 
            CreateQuestionRequest request, 
            AddQuestionValidator validator,
            CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id, ct);

        if (quiz is null)
        {
            return TypedResults.NotFound();
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
        await context.SaveChangesAsync(ct);
        
        var questionResponse = new CreateQuestionResponse
        (
            question.Id, 
            question.Text, 
            question.Type, 
            question.Answers.Select(a => new CreateAnswerResponse(a.Id, a.Text, a.IsCorrect)).ToList()
        );
        
        return TypedResults.Created($"/api/quizzes/{id}/questions/{question.Id}", questionResponse);
    }
}