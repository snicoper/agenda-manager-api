using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultGenericTests
{
    [Fact]
    public void Result_Generic_Success_Should_Return_Succeeded_True()
    {
        // Act
        var result = Result.Success<string>("Hello");

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public void Result_Generic_Failure_Empty_Should_Return_Succeeded_False()
    {
        // Arrange
        const ErrorType errorType = ErrorType.Conflict;

        // Act
        var result = Result.Failure<string>();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Generic_Failure_Should_Return_Succeeded_False()
    {
        // Arrange
        const ErrorType errorType = ErrorType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Failure_MapTo_Should_Change_Generic_Type_With_Empty_Value()
    {
        // Arrange
        const ErrorType errorType = ErrorType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.Succeeded.Should().BeFalse();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ErrorType.Should().Be(errorType);
        resultChanged.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Result_Success_MapTo_Should_Change_Generic_Type_With_Empty_Value()
    {
        // Arrange
        const ErrorType errorType = ErrorType.None;

        // Act
        var result = Result.Success(string.Empty);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.Succeeded.Should().BeTrue();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ErrorType.Should().Be(errorType);
        resultChanged.HasValue.Should().BeFalse();
    }
}
