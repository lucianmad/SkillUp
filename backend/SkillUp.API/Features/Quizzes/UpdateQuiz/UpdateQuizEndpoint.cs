using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.UpdateQuiz;

public static class UpdateQuizEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", HandleAsync);
    }

    private static async Task<Results<NoContent, NotFound, ValidationProblem>> HandleAsync(
        Guid id, 
        AppDbContext context, 
        QuizRequest request,
        UpdateQuizValidator validator,
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
        
        quiz.Title = request.Title;

        await context.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
    }
}