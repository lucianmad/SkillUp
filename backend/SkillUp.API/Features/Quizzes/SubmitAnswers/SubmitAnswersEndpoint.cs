using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkillUp.API.Database;

namespace SkillUp.API.Features.Quizzes.SubmitAnswers;

public static class SubmitAnswersEndpoint
{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/{quizId:guid}/submit", HandleAsync);
    }

    private static async Task<Results<Ok<SubmitAnswersResponse>, NotFound>> HandleAsync(
        Guid quizId,
        AppDbContext context,
        SubmitAnswersRequest request,
        CancellationToken ct)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == quizId, ct);
        if (quiz is null)
        {
            return TypedResults.NotFound();
        }

        var score = 0;
        var questionMap = quiz.Questions.ToDictionary(q => q.Id);

        foreach (var userAnswer in request.Answers.DistinctBy(a => a.QuestionId))
        {
            if (!questionMap.TryGetValue(userAnswer.QuestionId, out var question))
            {
                continue;
            }

            var correctIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id);

            var isCorrect = new HashSet<Guid>(correctIds).SetEquals(userAnswer.SelectedAnswersId);
            
            if (isCorrect)
            {
                score++;
            }
        }
        
        return TypedResults.Ok(new SubmitAnswersResponse(score, quiz.Questions.Count));
    }
}