using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ErrorTests
{
    private const string Code = "CodeError";
    private const string Description = "Description error";

    [Fact]
    public void Error_Validation()
    {
        // Act
        var error = Error.Validation(Code, Description);

        // Assert
        error.ErrorType.Should().Be(ErrorType.ValidationError);
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
        error.ErrorType.Should().Be(ErrorType.NotFound);
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
        error.ErrorType.Should().Be(ErrorType.Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ErrorType.Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Forbidden()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.ErrorType.Should().Be(ErrorType.Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ErrorType.Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }
}
