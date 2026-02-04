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
        var quiz = await context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == quizId, ct);

        if (quiz is null)
        {
            return TypedResults.NotFound();
        }
        
        var questions = quiz.Questions.Select(q => new QuestionResponse(
            q.Id,
            q.Text, 
            q.Type.ToString())).ToList();
        
        return TypedResults.Ok(questions);
    }
}