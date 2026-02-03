using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes.AddQuestion;

public record AnswerRequest(string Text, bool IsCorrect);
public record QuestionRequest(QuestionType Type, string Text, List<AnswerRequest> Answers);
    
public record AnswerResponse(Guid Id, string Text, bool IsCorrect);
public record QuestionResponse(Guid Id, string Text, QuestionType Type, List<AnswerResponse>? Answers = null);
