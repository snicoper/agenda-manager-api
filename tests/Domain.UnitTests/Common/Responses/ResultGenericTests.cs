using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultGenericTests
{
    [Fact]
    public void ResultGeneric_ShouldReturnSuccess_WhenSucceededIsTrue()
    {
        // Act
        var result = Result.Success<string>("Hello");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ResultGeneric_ShouldSuccessReturnFalse_WhenFailureIsSet()
    {
        // Arrange
        const ResultType errorType = ResultType.Conflict;

        // Act
        var result = Result.Failure<string>();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(errorType);
    }

    [Fact]
    public void ResultGeneric_ShouldReturnFailure_WhenSucceededIsFalse()
    {
        // Arrange
        const ResultType errorType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(errorType);
    }

    [Fact]
    public void ResultGeneric_ShouldReturnFailure_WhenChangeGenericTypeWithEmptyValue()
    {
        // Arrange
        const ResultType errorType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.IsSuccess.Should().BeFalse();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ResultType.Should().Be(errorType);
        resultChanged.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ResultGeneric_ShouldReturnSuccess_WhenMapToChangeGenericType()
    {
        // Arrange
        const ResultType errorType = ResultType.Succeeded;

        // Act
        var result = Result.Success<string>();
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.IsSuccess.Should().BeTrue();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ResultType.Should().Be(errorType);
        resultChanged.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ResultGeneric_ShouldReturnSuccess_WhenWhenRequiredGeneric()
    {
        // Act
        var result = Result.Success<string>();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
        result.Should().BeOfType<Result<string>>();
    }
}
