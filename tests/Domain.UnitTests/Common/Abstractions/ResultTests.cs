using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Domain.UnitTests.Common.Abstractions;

public class ResultTests
{
    [Fact]
    public void Result_Create_Empty_Should_Return_Succeeded_True()
    {
        // Arrange
        const int status = StatusCodes.Status200OK;

        // Act
        var result = Result.Create();

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Create_With_Status_Should_Return_Status()
    {
        // Arrange
        const int status = StatusCodes.Status201Created;

        // Act
        var result = Result.Create(status);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Success_Should_Return_Succeeded_True()
    {
        // Arrange
        const int status = StatusCodes.Status200OK;

        // Act
        var result = Result.Success();

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Success_With_Status_Should_Return_Status()
    {
        // Arrange
        const int status = StatusCodes.Status201Created;

        // Act
        var result = Result.Success(status);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Failure_Empty_Should_Return_Succeeded_False()
    {
        // Arrange
        const int status = StatusCodes.Status409Conflict;

        // Act
        var result = Result.Failure();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(status);
    }

    [Fact]
    public void Result_Failure_Should_Return_Succeeded_False()
    {
        // Arrange
        const int status = StatusCodes.Status403Forbidden;

        // Act
        var error = Error.Forbidden();
        var result = Result.Failure(error);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Status.Should().Be(status);
    }
}
