namespace SkillUp.API.Features.Quizzes.GetQuizById;

public record QuizDetailsResponse(Guid Id, string Title, int QuestionCount);