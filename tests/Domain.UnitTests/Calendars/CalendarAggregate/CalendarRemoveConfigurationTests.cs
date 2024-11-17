using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarRemoveConfigurationTests
{
    [Fact]
    public void RemoveConfiguration_ShouldRemove_WhenConfigurationExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();
        calendar.AddConfiguration(configuration);

        // Act
        calendar.RemoveConfiguration(configuration);

        // Assert
        calendar.Configurations.Should().BeEmpty();
    }

    [Fact]
    public void RemoveConfiguration_ShouldRaiseEvent_WhenConfigurationExist()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();
        calendar.AddConfiguration(configuration);

        // Act
        calendar.RemoveConfiguration(configuration);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarConfigurationRemovedDomainEvent);
    }
}
