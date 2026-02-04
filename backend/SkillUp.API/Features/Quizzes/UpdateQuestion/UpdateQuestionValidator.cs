using FluentValidation;
using SkillUp.API.Domain.Enums;

namespace SkillUp.API.Features.Quizzes.UpdateQuestion;

public class UpdateQuestionValidator: AbstractValidator<UpdateQuestionRequest>
{
    public UpdateQuestionValidator()
    {
        RuleFor(q => q.Text)
            .NotEmpty().WithMessage("Question text is required")
            .MaximumLength(255).WithMessage("Question text must not exceed 255 characters");
        RuleFor(q => q.Type)
            .IsInEnum().WithMessage("Question type is invalid");
        RuleFor(q => q.Answers)
            .NotEmpty().WithMessage("Question must have answers")
            .Must(q => q.Count >= 2).WithMessage("Question must have at least two answers");
        RuleFor(q => q.Answers)
            .Must(answers => answers.Any(a => a.IsCorrect))
            .WithMessage("Question must have at least one correct answer");
        When(q => q.Type == QuestionType.SingleChoice, () =>
        {
            RuleFor(q => q.Answers)
                .Must(answers => answers.Count(a => a.IsCorrect) == 1)
                .WithMessage("Single choice question must have only one correct answer");
        });
        RuleForEach(q => q.Answers).ChildRules(answer =>
        {
            answer.RuleFor(a => a.Text)
                .NotEmpty().WithMessage("Answer text is required")
                .MaximumLength(255).WithMessage("Answer text must not exceed 255 characters");
        });
    }
}