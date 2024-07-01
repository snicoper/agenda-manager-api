using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

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
        error.Status.Should().Be(StatusCodes.Status400BadRequest);
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
        error.Status.Should().Be(StatusCodes.Status404NotFound);
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
        error.Status.Should().Be(StatusCodes.Status401Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(StatusCodes.Status401Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }

    [Fact]
    public void Error_Forbidden()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.Status.Should().Be(StatusCodes.Status403Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(StatusCodes.Status403Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result>();
    }
}
