using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceUpdateScheduleTests
{
    [Fact]
    public void UpdateSchedule_ShouldSuccess_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule1 = ResourceScheduleFactory.CreateResourceSchedule();
        var schedule2 = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule1);
        resource.AddSchedule(schedule2);

        // Act
        var result = resource.UpdateSchedule(
            schedule1.Id,
            schedule2.Period,
            schedule2.Name,
            schedule2.Description,
            schedule2.AvailableDays);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void UpdateSchedule_ShouldNotFound_WhenScheduleIsNotFound()
    {
        // Arrange
        var expectedError = ResourceScheduleErrors.NotFound.FirstError();
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule);

        // Act
        var result = resource.UpdateSchedule(
            ResourceScheduleId.Create(),
            PeriodFactory.Create(),
            schedule.Name,
            schedule.Description,
            schedule.AvailableDays);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(expectedError);
    }

    [Theory]
    [InlineData(
        "Resource Schedule test",
        "Resource Schedule test description",
        WeekDays.Friday)] // availableDays is same.
    [InlineData(
        "Resource Schedule test",
        "Resource Schedule test description",
        WeekDays.WorkDays)] // All are same (Period is different).
    [InlineData("Resource Schedule test", "Description", WeekDays.WorkDays)] // All are same (Period is different).
    [InlineData("Resource Schedule test", "Description", WeekDays.All)] // name is same.
    [InlineData("Resource", "Resource Schedule test description", WeekDays.All)] // description is same.
    public void UpdateSchedule_ShouldRaiseEvent_WhenDataIsProvided(
        string name,
        string description,
        WeekDays availableDays)
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule);

        // Act
        resource.UpdateSchedule(
            schedule.Id,
            PeriodFactory.Create(),
            name,
            description,
            availableDays);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateSchedule_ShouldNotRaiseEvent_WhenSameValuesAreProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule);

        // Act
        resource.UpdateSchedule(
            schedule.Id,
            schedule.Period,
            schedule.Name,
            schedule.Description,
            schedule.AvailableDays);

        // Assert
        resource.DomainEvents.Should().NotContain(x => x is ResourceScheduleUpdatedDomainEvent);
    }
}
