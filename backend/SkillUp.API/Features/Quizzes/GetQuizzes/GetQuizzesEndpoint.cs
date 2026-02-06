using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.GetQuizzes;

public static class GetQuizzesEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync);
    }

    private static async Task<Ok<List<QuizResponse>>> HandleAsync(AppDbContext context, CancellationToken ct)
    {
        var quizzes = await context.Quizzes
            .AsNoTracking()
            .Select(q => new QuizResponse(q.Id, q.Title))
            .ToListAsync(ct);
        
        return TypedResults.Ok(quizzes);
    }
}