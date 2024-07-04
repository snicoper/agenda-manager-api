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
        error.ResultType.Should().Be(ResultType.Validation);
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
        error.ResultType.Should().Be(ResultType.NotFound);
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
        error.ResultType.Should().Be(ResultType.Unauthorized);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ResultType.Unauthorized));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Forbidden()
    {
        // Act
        var error = Error.Forbidden<ErrorTests>();

        // Assert
        error.ResultType.Should().Be(ResultType.Forbidden);
        error.HasErrors.Should().BeTrue();
        error.Code.Should().Be(nameof(ResultType.Forbidden));
        error.Description.Should().NotBeEmpty();
        error.ToResult().Should().BeOfType<Result<ErrorTests>>();
    }

    [Fact]
    public void Error_Generic_Validation_WhenDictionaryIdPassed()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Code", ["Description 1", "Description 2"] }, { "Code2", ["Description 3", "Description 4"] }
        };

        // Act
        var error = Error.Validation<ErrorTests>(errors);

        // Assert
        error.HasErrors.Should().BeTrue();
        error.ResultType.Should().Be(ResultType.Validation);
        error.ValidationErrors.Should().HaveCount(2);
        error.ValidationErrors.First().Value.Should().HaveCount(2);
        error.ValidationErrors.Last().Value.Should().HaveCount(2);
    }
}
