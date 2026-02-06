using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.GetQuestions;

public static class GetQuestionsEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{quizId:guid}/questions", HandleAsync);
    }

    private static async Task<Results<Ok<List<QuestionResponse>>, NotFound>> HandleAsync(
        Guid quizId,
        AppDbContext context,
        CancellationToken ct)
    {
        var quizExists = await context.Quizzes.AnyAsync(q => q.Id == quizId, ct);
        if (!quizExists)
        {
            return TypedResults.NotFound();
        }
        
        var questions = await context.Questions
            .AsNoTracking()
            .Where(q => q.QuizId == quizId)
            .Select(q => new QuestionResponse(q.Id, q.Text, q.Type.ToString()))
            .ToListAsync(ct);
        
        return TypedResults.Ok(questions);
    }
}