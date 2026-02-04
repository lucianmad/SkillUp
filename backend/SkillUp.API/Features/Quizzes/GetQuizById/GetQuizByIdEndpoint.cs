using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.GetQuizById;

public static class GetQuizByIdEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{id:Guid}", HandleAsync);
    }

    private static async Task<Results<Ok<QuizResponse>, NotFound>> HandleAsync(AppDbContext context, Guid id, CancellationToken ct)
    {
        var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id, ct);
        
        return quiz is null ? TypedResults.NotFound() : TypedResults.Ok(new QuizResponse(quiz.Id, quiz.Title));
    }
}