using SkillUp.API.Database;
using SkillUp.API.Domain;

namespace SkillUp.API.Features.Quizzes;

public static class PostQuiz
{
    public record QuizRequest(string Title);
    public record QuizResponse(Guid Id, string Title);

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync);
    }

    public static async Task<IResult> HandleAsync(AppDbContext context, QuizRequest request)
    {
        var quiz = new Quiz
        {
            Title = request.Title
        };

        context.Quizzes.Add(quiz);
        await context.SaveChangesAsync();
        
        var quizResponse = new QuizResponse(quiz.Id, quiz.Title);
        
        return Results.Created($"/api/quizzes/{quiz.Id}", quizResponse);
    } 
}