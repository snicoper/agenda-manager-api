using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class ResultGenericTests
{
    [Fact]
    public void Result_Generic_Create_Empty_Should_Return_Succeeded_True()
    {
        // Arrange
        const int status = StatusCodes.Status200OK;

        // Act
        var result = Result.Create<string>("Hello");

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Generic_Create_With_Status_Should_Return_Status()
    {
        // Arrange
        const int status = StatusCodes.Status201Created;

        // Act
        var result = Result.Create<string>("Hello", status);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Generic_Success_Should_Return_Succeeded_True()
    {
        // Arrange
        const int status = StatusCodes.Status200OK;

        // Act
        var result = Result.Success<string>("Hello");

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Generic_Success_With_Status_Should_Return_Status()
    {
        // Arrange
        const int status = StatusCodes.Status201Created;

        // Act
        var result = Result.Success<string>("Hello", status);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Generic_Failure_Empty_Should_Return_Succeeded_False()
    {
        // Arrange
        const int status = StatusCodes.Status409Conflict;

        // Act
        var result = Result.Failure<string>();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Generic_Failure_Should_Return_Succeeded_False()
    {
        // Arrange
        const int status = StatusCodes.Status403Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Failure_MapTo_Should_Change_Generic_Type_With_Empty_Value()
    {
        // Arrange
        const int status = StatusCodes.Status403Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure<string>(error);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.Succeeded.Should().BeFalse();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.Status.Should().Be(status);
        resultChanged.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Result_Success_MapTo_Should_Change_Generic_Type_With_Empty_Value()
    {
        // Arrange
        const int status = StatusCodes.Status200OK;

        // Act
        var result = Result.Success(string.Empty);
        var resultChanged = result.MapTo<int>();

        // Assert
        resultChanged.Succeeded.Should().BeTrue();
        resultChanged.Value.GetType().Should().Be(typeof(int));
        resultChanged.Status.Should().Be(status);
        resultChanged.HasValue.Should().BeFalse();
    }
}
