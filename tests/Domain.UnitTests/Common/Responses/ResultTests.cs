using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultTests
{
    [Fact]
    public void Result_ShouldReturnSuccess_WhenSucceeded()
    {
        // Arrange
        const ResultType resultType = ResultType.Succeeded;

        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(resultType);
    }

    [Fact]
    public void Result_ShouldReturnFailure_WhenFailure()
    {
        // Arrange
        const ResultType resultType = ResultType.Conflict;

        // Act
        var result = Result.Failure();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(resultType);
    }

    [Fact]
    public void Result_ShouldReturnForbidden_WhenForbidden()
    {
        // Arrange
        const ResultType resultType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(resultType);
    }

    [Fact]
    public void Result_ShouldReturnSuccess_WhenMapToValue()
    {
        // Arrange
        var error = Result.Success();

        // Act
        var result = error.MapToValue<string>("test");

        // Assert
        result.Should().BeOfType<Result<string>>();
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
        result.Value.Should().Be("test");
    }

    [Fact]
    public void Result_ShouldReturnFailure_WhenMapToValue()
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

    [Fact]
    public void Result_ShouldReturnCreated_WhenCreate()
    {
        // Arrange
        var error = Result.Create();

        // Act
        var result = error.MapToValue<string>("test");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Created);
        result.Value.Should().Be("test");
    }

    [Fact]
    public void Result_ShouldReturnNoContent()
    {
        // Arrange

        // Act
        var result = Result.NoContent();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.NoContent);
    }
}
