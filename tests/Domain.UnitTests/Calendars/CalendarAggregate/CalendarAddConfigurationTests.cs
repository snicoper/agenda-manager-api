using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarAddConfigurationTests
{
    [Fact]
    public void AddConfiguration_ShouldAdd_WhenValidConfigurationProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Act
        calendar.AddConfiguration(configuration);

        // Assert
        calendar.Configurations.Should().Contain(configuration);
    }

    [Fact]
    public void AddConfiguration_ShouldRaiseEvent_WhenValidConfigurationProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Act
        calendar.AddConfiguration(configuration);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarConfigurationAddedDomainEvent);
    }
}
