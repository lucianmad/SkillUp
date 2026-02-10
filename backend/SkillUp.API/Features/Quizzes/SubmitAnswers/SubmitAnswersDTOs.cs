namespace SkillUp.API.Features.Quizzes.SubmitAnswers;

public record SubmitAnswerDTO(Guid QuestionId, List<Guid> SelectedAnswersId);
public record SubmitAnswersRequest(List<SubmitAnswerDTO> Answers);
public record SubmitAnswersResponse(int Score, int TotalQuestions);