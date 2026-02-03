using FluentValidation;

namespace SkillUp.API.Features.Quizzes.CreateQuiz;

public class CreateQuizValidator: AbstractValidator<QuizRequest>
{
    public CreateQuizValidator()
    {
        RuleFor(q => q.Title)
            .NotEmpty().WithMessage("Quiz title is required")
            .MaximumLength(200).WithMessage("Quiz title must not exceed 200 characters");
    }
}