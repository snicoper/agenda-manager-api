using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ErrorTests
{
    private const string Code = "CodeError";
    private const string Description = "Description error";

    [Fact]
    public void Error_None_Should_Return_Succeeded()
    {
        // Act
        var error = Error.None();

        // Assert
        error.ResultType.Should().Be(ResultType.Succeeded);
        error.HasErrors.Should().BeFalse();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Validation()
    {
        // Act
        var error = Error.Validation(Code, Description);

        // Assert
        error.ResultType.Should().Be(ResultType.Validation);
        error.HasErrors.Should().BeTrue();
        error.ValidationErrors.Should().HaveCount(1);
        error.ValidationErrors.First().Key.Should().Be(Code.ToLowerFirstLetter());
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Validation_Should_Group_By_Code()
    {
        // Arrange
        const string code2 = "Code2";

        // Act
        var error = Error.Validation(Code, Description);
        error.AddValidationError(Code, Description);
        error.AddValidationError(Code, Description);
        error.AddValidationError(Code, Description);
        error.AddValidationError(code2, Description);
        error.AddValidationError(code2, Description);

        // Assert
        error.ValidationErrors.First().Value.Should().HaveCount(4);
        error.ValidationErrors.Last().Value.Should().HaveCount(2);
    }

    [Fact]
    public void Error_NotFound()
    {
        // Act
        var error = Error.NotFound(Code, Description);

        // Assert
        error.ResultType.Should().Be(ResultType.NotFound);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(Code);
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Unauthorized()
    {
        // Act
        var error = Error.Unauthorized();

        // Assert
        error.ResultType.Should().Be(ResultType.Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ResultType.Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Forbidden()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.ResultType.Should().Be(ResultType.Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ResultType.Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Validation_WhenDictionaryIdPassed()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Code", ["Description 1", "Description 2"] }, { "Code2", ["Description 3", "Description 4"] }
        };

        // Act
        var error = Error.Validation(errors);

        // Assert
        error.HasErrors.Should().BeTrue();
        error.ResultType.Should().Be(ResultType.Validation);
        error.ValidationErrors.Should().HaveCount(2);
        error.ValidationErrors.First().Value.Should().HaveCount(2);
        error.ValidationErrors.Last().Value.Should().HaveCount(2);
    }

    [Fact]
    public void Error_Conflict_ShouldSetResultTypeToConflict()
    {
        // Act
        var error = Error.Conflict();

        // Assert
        error.ResultType.Should().Be(ResultType.Conflict);
        error.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Error_Unexpected_ShouldSetResultTypeToUnexpected()
    {
        // Act
        var error = Error.Unexpected(null);

        // Assert
        error.ResultType.Should().Be(ResultType.Unexpected);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ResultType.Unexpected));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ToResult_ShouldReturnResult_WhenImplicitCall()
    {
        // Arrange
        var error = Error.None();

        // Act
        Result result = error;

        // Assert
        result.Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ToResult_ShouldReturnResult()
    {
        // Arrange
        var error = Error.None();

        // Act
        var result = error.ToResult();

        // Assert
        result.Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_ToResultGeneric_ShouldReturnResultWithResultType()
    {
        // Arrange
        var error = Error.Unauthorized();

        // Act
        var result = error.ToResult<ErrorTests>();

        // Assert
        result.Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_HasErrors_ShouldReturnFalse_WhenNoContainsErrors()
    {
        // Act
        var error = Error.None();

        // Assert
        error.HasErrors.Should().BeFalse();
    }

    [Fact]
    public void Error_HasErrors_ShouldReturnTrue_WhenValidationErrorsContainsErrors()
    {
        // Act
        var error = Error.Validation(Code, Description);

        // Assert
        error.HasErrors.Should().BeTrue();
    }

    [Fact]
    public void Error_HasErrors_ShouldReturnTrue_WhenCodeContainsErrors()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.HasErrors.Should().BeTrue();
    }
}
