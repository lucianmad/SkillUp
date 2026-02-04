using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.DeleteQuiz;

public static class DeleteQuizEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", HandleAsync);
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(Guid id, AppDbContext context, CancellationToken ct)
    {
        var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id, ct);

        if (quiz is null)
        {
            return TypedResults.NotFound();
        }
        
        context.Quizzes.Remove(quiz);
        await context.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
    }
}