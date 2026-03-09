namespace SkillUp.API.Features.Quizzes.GetQuestions;

public record AnswerListResponse(Guid Id, string Text);
public record QuestionListResponse(Guid Id, string Text, string Type, List<AnswerListResponse>? Answers = null);