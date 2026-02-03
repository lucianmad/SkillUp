using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Domain;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;
    public List<Answer> Answers { get; set; } = new();
}