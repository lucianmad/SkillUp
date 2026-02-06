using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.DeleteQuestion;

public static class DeleteQuestionEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{quizId:guid}/questions/{questionId:guid}", HandleAsync);
    }

    private static async Task<Results<NoContent, NotFound>> HandleAsync(
        Guid quizId,
        Guid questionId,
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
        
        var question = quiz.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
        {
            return TypedResults.NotFound();
        }
        
        quiz.Questions.Remove(question);
        await context.SaveChangesAsync(ct);
        
        return TypedResults.NoContent();
        
    }
}