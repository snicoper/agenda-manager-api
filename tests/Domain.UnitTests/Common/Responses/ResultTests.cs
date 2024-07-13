using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultTests
{
    [Fact]
    public void Result_Create_Empty_Should_Return_Succeeded_True()
    {
        // Arrange
        const ResultType errorType = ResultType.Succeeded;

        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Failure_Empty_Should_Return_Succeeded_False()
    {
        // Arrange
        const ResultType errorType = ResultType.Conflict;

        // Act
        var result = Result.Failure();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(errorType);
    }

    [Fact]
    public void Result_Failure_Should_Return_Succeeded_False()
    {
        // Arrange
        const ResultType errorType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(errorType);
    }

    [Fact]
    public void Result_ShouldReturnSuccess_WhenMapToGeneric()
    {
        // Arrange
        var error = Result.Success();

        // Act
        var result = error.MapToValue<string>("test");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
        result.Value.Should().Be("test");
    }

    [Fact]
    public void Result_ShouldReturnFailure_WhenMapToGeneric()
    {
        // Arrange
        var error = Result.Failure();

        // Act
        var result = error.MapToValue<string>("test");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(ResultType.Conflict);
        result.Value.Should().Be("test");
    }
}
