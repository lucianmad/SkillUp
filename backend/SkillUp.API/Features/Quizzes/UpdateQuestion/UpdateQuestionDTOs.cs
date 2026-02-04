using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes.UpdateQuestion;

public record UpdateAnswerRequest(string Text, bool IsCorrect);
public record UpdateQuestionRequest(QuestionType Type, string Text, List<UpdateAnswerRequest> Answers);
    
public record UpdateAnswerResponse(Guid Id, string Text, bool IsCorrect);
public record UpdateQuestionResponse(Guid Id, string Text, QuestionType Type, List<UpdateAnswerResponse>? Answers = null);