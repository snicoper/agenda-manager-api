using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultTests
{
    [Fact]
    public void Result_Create_Empty_Should_Return_Succeeded_True()
    {
        // Arrange
        const ErrorType errorType = ErrorType.None;

        // Act
        var result = Result.Create();

        // Assert
        result.Succeeded.Should().BeTrue();
        result.ErrorType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Failure_Empty_Should_Return_Succeeded_False()
    {
        // Arrange
        const ErrorType errorType = ErrorType.Conflict;

        // Act
        var result = Result.Failure();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Failure_Should_Return_Succeeded_False()
    {
        // Arrange
        const ErrorType errorType = ErrorType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure(error);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.ErrorType.Should().Be(errorType);
    }
}
