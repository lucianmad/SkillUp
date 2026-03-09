namespace SkillUp.API.Features.Quizzes.CreateQuiz;

public record CreateQuizRequest(string Title);
public record CreateQuizResponse(Guid Id, string Title);