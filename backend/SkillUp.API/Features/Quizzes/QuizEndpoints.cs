namespace SkillUp.API.Features.Quizzes;

public static class QuizEndpoints
{
    public static void MapQuizEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/quizzes")
            .WithTags("Quizzes");
        
        GetQuizzes.GetQuizzesEndpoint.MapEndpoint(group);
        CreateQuiz.CreateQuizEndpoint.MapEndpoint(group);
        GetQuizById.GetQuizByIdEndpoint.MapEndpoint(group);
        UpdateQuiz.UpdateQuizEndpoint.MapEndpoint(group);
        DeleteQuiz.DeleteQuizEndpoint.MapEndpoint(group);
        AddQuestion.AddQuestionEndpoint.MapEndpoint(group);
        UpdateQuestion.UpdateQuestionEndpoint.MapEndpoint(group);
        GetQuestions.GetQuestionsEndpoint.MapEndpoint(group);
        DeleteQuestion.DeleteQuestionEndpoint.MapEndpoint(group);
    }
}