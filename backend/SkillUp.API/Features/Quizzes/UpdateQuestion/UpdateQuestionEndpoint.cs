using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;
using SkillUp.API.Domain;

namespace SkillUp.API.Features.Quizzes.UpdateQuestion;

public static class UpdateQuestionEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{quizId:guid}/questions/{questionId:guid}", HandleAsync);
    }
    
    private static async Task<Results<NoContent, NotFound, ValidationProblem>> HandleAsync(
        Guid quizId,
        Guid questionId,
        AppDbContext context, 
        UpdateQuestionRequest request, 
        UpdateQuestionValidator validator,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        var quiz = await context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == quizId, ct);

        if (quiz is null)
        {
            return TypedResults.NotFound();
        }
        
        var question = quiz.Questions.FirstOrDefault(q => q.Id == questionId);

        if (question is null)
        {
            return TypedResults.NotFound();
        }

        question.Text = request.Text;
        question.Type = request.Type;
        
        context.RemoveRange(question.Answers);

        var newAnswers = request.Answers.Select(a => new Answer
        {
            IsCorrect = a.IsCorrect,
            Text = a.Text,
            QuestionId = questionId
        });
        
        context.AddRange(newAnswers);
        
        await context.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
    }

}