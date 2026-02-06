using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.GetQuizById;

public static class GetQuizByIdEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", HandleAsync);
    }

    private static async Task<Results<Ok<QuizResponse>, NotFound>> HandleAsync(AppDbContext context, Guid id, CancellationToken ct)
    {
        var quiz = await context.Quizzes
            .AsNoTracking()
            .Where(q => q.Id == id)
            .Select(q => new QuizResponse(q.Id, q.Title, q.Questions.Count))
            .FirstOrDefaultAsync(ct);
        
        return quiz is null ? TypedResults.NotFound() : TypedResults.Ok(quiz);
    }
}