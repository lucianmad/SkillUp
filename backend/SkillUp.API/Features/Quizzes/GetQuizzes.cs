using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes;

public static class GetQuizzes
{
    private record QuizResponse(Guid Id, string Title);
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(AppDbContext context)
    {
        var quizzes = await context.Quizzes
            .Select(q => new QuizResponse(q.Id, q.Title))
            .ToListAsync();
        
        return Results.Ok(quizzes);
    }
}