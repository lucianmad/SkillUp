using FluentValidation.TestHelper;
using SkillUp.API.Features.Quizzes.CreateQuiz;

namespace SkillUp.Tests.Features.Quizzes.CreateQuiz;

public class CreateQuizValidatorTests
{
    private readonly CreateQuizValidator _validator = new();
    
    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var request = new QuizRequest("Integration testing");
        
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var request = new QuizRequest(string.Empty);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Title)
            .WithErrorMessage("Quiz title is required");
    }
    
    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        var request = new QuizRequest(new string('a', 201));
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Title)
            .WithErrorMessage("Quiz title must not exceed 200 characters");
    }
}