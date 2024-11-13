using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ErrorTests
{
    private const string Code = "CodeError";
    private const string Description = "Description error";

    [Fact]
    public void Error_ShouldReturnSucceeded_WhenNone()
    {
        // Act
        var error = Error.None();

        // Assert
        error.ResultType.Should().Be(ResultType.Succeeded);
        error.HasErrors.Should().BeFalse();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnValidationError_WhenValidation()
    {
        // Act
        var error = Error.Validation(Code, Description);

        // Assert
        error.ResultType.Should().Be(ResultType.Validation);
        error.HasErrors.Should().BeTrue();
        error.ValidationErrors.Should().HaveCount(1);
        error.FirstError()?.Code.Should().Be(Code);
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldGroupedByCode_WhenValidation()
    {
        // Arrange
        const string code2 = "Code2";

        // Act
        var error = Error.Validation(Code, Description);
        error = error.AddValidationError(Code, Description);
        error = error.AddValidationError(Code, Description);
        error = error.AddValidationError(Code, Description);
        error = error.AddValidationError(code2, Description);
        error = error.AddValidationError(code2, Description);

        // Assert
        error.ToDictionary().First().Value.Should().HaveCount(4);
        error.ToDictionary().Last().Value.Should().HaveCount(2);
    }

    [Fact]
    public void Error_ShouldReturnNotFound_WhenNotFound()
    {
        // Act
        var error = Error.NotFound(Description);

        // Assert
        error.ResultType.Should().Be(ResultType.NotFound);
        error.HasErrors.Should().BeTrue();
        error.FirstError()?.Code.Should().Be("NotFound");
        error.FirstError()?.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnUnauthorized_WhenUnauthorized()
    {
        // Act
        var error = Error.Unauthorized();

        // Assert
        error.ResultType.Should().Be(ResultType.Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.FirstError()?.Code.Should().Be(nameof(ResultType.Unauthorized));
        error.FirstError()?.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnForbidden_WhenForbidden()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.ResultType.Should().Be(ResultType.Forbidden);
        error.HasErrors.Should().BeTrue();
        error.FirstError()?.Code.Should().Be(nameof(ResultType.Forbidden));
        error.FirstError()?.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnValidation_WhenDictionaryIdPassed()
    {
        // Arrange
        List<ValidationError> errors =
        [
            new("Code", "Description 1"),
            new("Code", "Description 2"),
            new("Code2", "Description 3"),
            new("Code2", "Description 4")
        ];

        // Act
        var error = Error.Validation(errors);

        // Assert
        error.HasErrors.Should().BeTrue();
        error.ResultType.Should().Be(ResultType.Validation);
        error.ValidationErrors.Should().HaveCount(4);
        error.ToDictionary().First().Value.Should().HaveCount(2);
        error.ToDictionary().Last().Value.Should().HaveCount(2);
    }

    [Fact]
    public void Error_ShouldSetResultTypeToConflict_WhenConflict()
    {
        // Act
        var error = Error.Conflict();

        // Assert
        error.ResultType.Should().Be(ResultType.Conflict);
        error.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Error_ShouldSetResultTypeToUnexpected_WhenUnexpected()
    {
        // Act
        var error = Error.Unexpected();

        // Assert
        error.ResultType.Should().Be(ResultType.Unexpected);
        error.HasErrors.Should().BeTrue();
        error.FirstError()?.Code.Should().Be(nameof(ResultType.Unexpected));
        error.FirstError()?.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnResult_WhenImplicitCall()
    {
        // Arrange
        var error = Error.None();

        // Act
        Result result = error;

        // Assert
        result.Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnResult_WhenToResult()
    {
        // Arrange
        var error = Error.None();

        // Act
        var result = error.ToResult();

        // Assert
        result.Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ShouldReturnResultWithResultType_WhenToResultGeneric()
    {
        // Arrange
        var error = Error.Unauthorized();

        // Act
        var errorResult = error.ToResult<ErrorTests>();

        // Assert
        errorResult.Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_ShouldHasErrorsReturnFalse_WhenNoContainsErrors()
    {
        // Act
        var error = Error.None();

        // Assert
        error.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void Error_ShouldHasErrorsReturnTrue_WhenValidationErrorsContainsErrors()
    {
        // Act
        var error = Error.Validation(Code, Description);

        // Assert
        error.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Error_ShouldHasErrorsReturnTrue_WhenCodeContainsErrors()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.HasErrors.Should().BeTrue();
    }
}
