using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes.UpdateQuestion;

public record UpdateAnswerRequest(string Text, bool IsCorrect);
public record UpdateQuestionRequest(QuestionType Type, string Text, List<UpdateAnswerRequest> Answers);