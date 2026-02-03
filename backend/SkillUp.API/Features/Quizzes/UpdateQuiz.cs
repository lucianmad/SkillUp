using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes;

public static class UpdateQuiz
{
    public record QuizRequest(string Title);
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", HandleAsync);
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        Guid id, 
        AppDbContext context, 
        QuizRequest request,
        CancellationToken ct)
    {
        var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id, ct);

        if (quiz == null)
        {
            return TypedResults.NotFound();
        }
        
        quiz.Title = request.Title;

        await context.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
    }
}