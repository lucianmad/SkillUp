namespace SkillUp.API.Domain;

public class Quiz
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new ();
}

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public Guid QuizId { get; set; }
    
    public Quiz Quiz { get; set; }
    public List<Answer> Answers { get; set; } = new();
}

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
    
    public Question Question { get; set; }
}

public enum QuestionType
{
    SingleChoice,
    MultipleChoice
}