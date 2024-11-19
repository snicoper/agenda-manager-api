using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Appointments.ValueObjects;

public class AppointmentCurrentStateTests
{
    [Fact]
    public void CurrentState_ShouldSuccess_WithFromValidValue()
    {
        // Act
        var state = AppointmentCurrentState.From(AppointmentStatus.Accepted);

        // Assert
        state.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    public void CurrentState_ShouldSuccess_WithCreateValidValue(AppointmentStatus status)
    {
        // Act
        var state = AppointmentCurrentState.Create(status);

        // Assert
        state.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithCreateInvalidValue(AppointmentStatus status)
    {
        // Act
        var state = AppointmentCurrentState.Create(status);

        // Assert
        state.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CurrentState_ShouldSuccess_WithToPendingValidValue()
    {
        // Arrange
        var state = AppointmentCurrentState.From(AppointmentStatus.RequiresRescheduling);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Pending);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Accepted)]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToPendingInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Pending);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    public void CurrentState_ShouldSuccess_WithToAcceptedValidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Accepted);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Accepted)]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToAcceptedInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Pending);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    public void CurrentState_ShouldSuccess_WithToWaitingValidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Waiting);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    public void CurrentState_ShouldFailure_WithToWaitingInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Waiting);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    public void CurrentState_ShouldSuccess_WithToCancelledValidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Cancelled);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToCancelledInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Cancelled);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    public void CurrentState_ShouldSuccess_WithToRequiresReschedulingValidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Cancelled);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.Cancelled)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToRequiresReschedulingInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.RequiresRescheduling);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CurrentState_ShouldSuccess_WithToInProgressValidValue()
    {
        // Arrange
        var state = AppointmentCurrentState.From(AppointmentStatus.Waiting);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.InProgress);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    [InlineData(AppointmentStatus.InProgress)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToInProgressInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.InProgress);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void CurrentState_ShouldSuccess_WithToCompletedValidValue()
    {
        // Arrange
        var state = AppointmentCurrentState.From(AppointmentStatus.InProgress);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Completed);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Accepted)]
    [InlineData(AppointmentStatus.Waiting)]
    [InlineData(AppointmentStatus.RequiresRescheduling)]
    [InlineData(AppointmentStatus.Completed)]
    public void CurrentState_ShouldFailure_WithToCompletedInvalidValue(AppointmentStatus status)
    {
        // Arrange
        var state = AppointmentCurrentState.From(status);

        // Act
        var result = state.Value!.ChangeStatus(AppointmentStatus.Completed);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
