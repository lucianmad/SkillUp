using FluentValidation;

namespace SkillUp.API.Features.Quizzes.UpdateQuiz;

public class UpdateQuizValidator: AbstractValidator<QuizRequest>
{
    public UpdateQuizValidator()
    {
        RuleFor(q => q.Title)
            .NotEmpty().WithMessage("Quiz title is required")
            .MaximumLength(200).WithMessage("Quiz title must not exceed 200 characters");
    }
}