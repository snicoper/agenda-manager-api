using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ErrorGenericTests
{
    private const string Code = "CodeError";
    private const string Description = "Description error";

    [Fact]
    public void Error_Generic_Validation()
    {
        // Act
        var error = Error.Validation<ErrorTests>(Code, Description);

        // Assert
        error.ErrorType.Should().Be(ErrorType.ValidationError);
        error.HasErrors.Should().BeTrue();
        error.ValidationErrors.Should().HaveCount(1);
        error.ValidationErrors.First().Key.Should().Be(Code.ToLowerFirstLetter());
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Validation_Should_Group_By_Code()
    {
        // Act
        var error = Error.Validation<string>(Code, Description);
        error.AddValidationError(Code, Description);
        error.AddValidationError(Code, Description);
        error.AddValidationError(Code, Description);

        // Assert
        error.ValidationErrors.First().Value.Should().HaveCount(4);
    }

    [Fact]
    public void Error_Generic_NotFound()
    {
        // Act
        var error = Error.NotFound<ErrorTests>(Code, Description);

        // Assert
        error.ErrorType.Should().Be(ErrorType.NotFound);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(Code);
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Unauthorized()
    {
        // Act
        var error = Error.Unauthorized<ErrorTests>();

        // Assert
        error.ErrorType.Should().Be(ErrorType.Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ErrorType.Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Forbidden()
    {
        // Act
        var error = Error.Forbidden<ErrorTests>();

        // Assert
        error.ErrorType.Should().Be(ErrorType.Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ErrorType.Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }
}
