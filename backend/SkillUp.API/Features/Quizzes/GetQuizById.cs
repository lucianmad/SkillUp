using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes;

public static class GetQuizById
{
    public record QuizResponse(Guid Id, string Title);

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{id:Guid}", HandleAsync);
    }

    private static async Task<IResult> HandleAsync(AppDbContext context, Guid id)
    {
        var quiz = await context.Quizzes.FindAsync(id);
        
        return quiz is null ? Results.NotFound() : Results.Ok(new QuizResponse(quiz.Id, quiz.Title));
    }
}