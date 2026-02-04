using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes.AddQuestion;

public record CreateAnswerRequest(string Text, bool IsCorrect);
public record CreateQuestionRequest(QuestionType Type, string Text, List<CreateAnswerRequest> Answers);
    
public record CreateAnswerResponse(Guid Id, string Text, bool IsCorrect);
public record CreateQuestionResponse(Guid Id, string Text, QuestionType Type, List<CreateAnswerResponse>? Answers = null);
