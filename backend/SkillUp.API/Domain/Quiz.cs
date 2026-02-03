namespace SkillUp.API.Domain;

public class Quiz
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new ();
}