namespace SkillUp.API.Features.Quizzes;

public static class QuizEndpoints
{
    public static void MapQuizEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/quizzes")
            .WithTags("Quizzes");
        
        GetQuizzes.MapEndpoint(group);
        CreateQuiz.CreateQuizEndpoint.MapEndpoint(group);
        GetQuizById.MapEndpoint(group);
        UpdateQuiz.MapEndpoint(group);
        DeleteQuiz.MapEndpoint(group);
        AddQuestion.AddQuestionEndpoint.MapEndpoint(group);
    }
}