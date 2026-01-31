using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes;

public static class PutQuiz
{
    public record QuizRequest(string Title);
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(Guid id, AppDbContext context, QuizRequest request)
    {
        var quiz = await context.Quizzes.FindAsync(id);

        if (quiz == null)
        {
            return Results.NotFound();
        }
        
        quiz.Title = request.Title;

        await context.SaveChangesAsync();
        
        return Results.NoContent();
    }
}