using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Responses;

public class ResultGenericTests
{
    [Fact]
    public void ResultGeneric_ShouldReturnSuccess_WhenResultIsSuccess()
    {
        // Act
        var result = Result.Success<string>();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Succeeded);
        result.Should().BeOfType<Result<string>>();
    }

    [Fact]
    public void ResultGeneric_ShouldIsSuccessReturnFalse_WhenFailureIsTrue()
    {
        // Arrange
        const ResultType resultType = ResultType.Conflict;

        // Act
        var result = Result.Failure<string>();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(resultType);
    }

    [Fact]
    public void ResultGeneric_ShouldReturnCreated_WhenCreatedIsSet()
    {
        // Act
        var result = Result.Create<string>("test");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ResultType.Should().Be(ResultType.Created);
        result.Value.Should().Be("test");
    }

    [Fact]
    public void ResultGeneric_ShouldReturnIsSuccessFalse_WhenForbiddenIsSet()
    {
        // Arrange
        const ResultType resultType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ResultType.Should().Be(resultType);
    }

    [Fact]
    public void ResultGeneric_ShouldReturnForbidden_WhenMapToChangeGenericType()
    {
        // Arrange
        const ResultType resultType = ResultType.Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.IsSuccess.Should().BeFalse();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ResultType.Should().Be(resultType);
        resultChanged.HasValue.Should().BeFalse();
    }

    [Fact]
    public void ResultGeneric_ShouldReturnSuccess_WhenMapToChangeGenericType()
    {
        // Arrange
        const ResultType resultType = ResultType.Succeeded;

        // Act
        var result = Result.Success<string>();
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.IsSuccess.Should().BeTrue();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.ResultType.Should().Be(resultType);
        resultChanged.HasValue.Should().BeFalse();
    }
}
