using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;
using SkillUp.API.Domain;

namespace SkillUp.API.Features.Quizzes.CreateQuiz;

public static class CreateQuizEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleAsync);
    }
    
    private static async Task<Results<Created<QuizResponse>, Conflict<string>, ValidationProblem>> HandleAsync(
            AppDbContext context, 
            QuizRequest request, 
            CreateQuizValidator validator,
            CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        var quizTitleExists = await context.Quizzes.AnyAsync(q => q.Title == request.Title, ct);
        if (quizTitleExists)
        {
            return TypedResults.Conflict($"A quiz with title '{request.Title}' already exists.");
        }
        
        var quiz = new Quiz
        {
            Title = request.Title
        };

        context.Quizzes.Add(quiz);
        await context.SaveChangesAsync(ct);
        
        var quizResponse = new QuizResponse(quiz.Id, quiz.Title);
        
        return TypedResults.Created($"/api/quizzes/{quiz.Id}", quizResponse);
    } 
}