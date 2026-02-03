namespace SkillUp.API.Features.Quizzes.CreateQuiz;

public record QuizRequest(string Title);
public record QuizResponse(Guid Id, string Title);