namespace SkillUp.API.Features.Quizzes.SubmitAnswers;

public record SubmitAnswerItem(Guid QuestionId, List<Guid> SelectedAnswersId);
public record SubmitAnswersRequest(List<SubmitAnswerItem> Answers);
public record SubmitAnswersResponse(int Score, int TotalQuestions);