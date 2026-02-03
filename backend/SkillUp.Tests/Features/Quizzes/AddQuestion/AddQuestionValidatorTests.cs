using FluentValidation.TestHelper;
using SkillUp.API.Domain.Enums;
using SkillUp.API.Features.Quizzes.AddQuestion;

namespace SkillUp.Tests.Features.Quizzes.AddQuestion;

public class AddQuestionValidatorTests
{
    private readonly AddQuestionValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var firstAnswer = new AnswerRequest("First answer", true);
        var secondAnswer = new AnswerRequest("Second answer", false);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [firstAnswer, secondAnswer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_MultipleChoice_Question_Has_More_Than_One_Correct_Answer()
    {
        var firstAnswer = new AnswerRequest("First answer", true);
        var secondAnswer = new AnswerRequest("Second answer", true);
        var request = new QuestionRequest(QuestionType.MultipleChoice, "Test question", [firstAnswer, secondAnswer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Should_Have_Error_When_Text_Is_Empty()
    {
        var request = new QuestionRequest(QuestionType.SingleChoice, "", []);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Text)
            .WithErrorMessage("Question text is required");
    }
    
    [Fact]
    public void Should_Have_Error_When_Text_Is_Too_Long()
    {
        var request = new QuestionRequest(QuestionType.SingleChoice, new string('a', 256), []);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Text)
            .WithErrorMessage("Question text must not exceed 255 characters");
    }

    [Fact]
    public void Should_Have_Error_When_Type_Is_Invalid_Enum()
    {
        var request = new QuestionRequest((QuestionType)100, "Test question", []);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Type)
            .WithErrorMessage("Question type is invalid");
    }

    [Fact]
    public void Should_Have_Error_When_Answers_Are_Empty()
    {
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", []);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Answers)
            .WithErrorMessage("Question must have answers");
    }

    [Fact]
    public void Should_Have_Error_When_Answers_Have_Less_Than_Two_Questions()
    {
        var answer = new AnswerRequest("Test answer", false);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [answer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Answers)
            .WithErrorMessage("Question must have at least two answers");
    }
    
    [Fact]
    public void Should_Have_Error_When_Answers_Have_No_Correct_Answer()
    {
        var answer = new AnswerRequest("Test answer", false);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [answer, answer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Answers)
            .WithErrorMessage("Question must have at least one correct answer");
    }
    
    [Fact]
    public void Should_Have_Error_When_SingleChoice_Question_Has_More_Than_One_Correct_Answer()
    {
        var answer = new AnswerRequest("Test answer", true);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [answer, answer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor(q => q.Answers)
            .WithErrorMessage("Single choice question must have only one correct answer");
    }
    
    [Fact]
    public void Should_Have_Error_When_Answer_Text_Is_Empty()
    {
        var firstAnswer = new AnswerRequest(string.Empty, true);
        var secondAnswer = new AnswerRequest("Second answer", false);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [firstAnswer, secondAnswer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor("Answers[0].Text")
            .WithErrorMessage("Answer text is required");
    }
    
    [Fact]
    public void Should_Have_Error_When_Answer_Text_Is_Too_Long()
    {
        var firstAnswer = new AnswerRequest(new string('a', 256), true);
        var secondAnswer = new AnswerRequest("Second answer", false);
        var request = new QuestionRequest(QuestionType.SingleChoice, "Test question", [firstAnswer, secondAnswer]);
        
        var result = _validator.TestValidate(request);
        
        result.ShouldHaveValidationErrorFor("Answers[0].Text")
            .WithErrorMessage("Answer text must not exceed 255 characters");
    }
}