using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes;

public static class DeleteQuiz
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid id, AppDbContext context)
    {
        var quiz = await context.Quizzes.FindAsync(id);

        if (quiz == null)
        {
            return Results.NotFound();
        }
        
        context.Quizzes.Remove(quiz);
        await context.SaveChangesAsync();
        
        return Results.NoContent();
    }
}