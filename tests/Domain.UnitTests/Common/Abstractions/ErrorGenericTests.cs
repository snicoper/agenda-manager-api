using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

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
        error.Status.Should().Be(StatusCodes.Status400BadRequest);
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
        error.Status.Should().Be(StatusCodes.Status404NotFound);
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
        error.Status.Should().Be(StatusCodes.Status401Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(StatusCodes.Status401Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Forbidden()
    {
        // Act
        var error = Error.Forbidden<ErrorTests>();

        // Assert
        error.Status.Should().Be(StatusCodes.Status403Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(StatusCodes.Status403Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Unknown()
    {
        // Act
        var error = Error.Unknown<ErrorTests>(default);

        // Assert
        error.Status.Should().Be(StatusCodes.Status500InternalServerError);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(StatusCodes.Status500InternalServerError));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }
}
