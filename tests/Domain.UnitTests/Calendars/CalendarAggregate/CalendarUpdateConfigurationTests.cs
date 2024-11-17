using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarUpdateConfigurationTests
{
    [Fact]
    public void UpdateConfiguration_ShouldUpdate_WhenConfigurationExists()
    {
        // Arrange
        const string initialSelectedKey = CalendarConfigurationKeys.Appointments.CreationOptions.RequireConfirmation;
        const string newSelectedKey = CalendarConfigurationKeys.Appointments.CreationOptions.Direct;
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(selectedKey: initialSelectedKey);
        calendar.AddConfiguration(configuration);

        // Act
        calendar.UpdateConfiguration(
            configurationId: configuration.Id,
            category: CalendarConfigurationKeys.Appointments.CreationStrategy,
            selectedKey: newSelectedKey);

        // Assert
        calendar.Configurations.First().SelectedKey.Should().Be(newSelectedKey);
    }

    [Fact]
    public void UpdateConfiguration_ShouldRaiseEvent_WhenConfigurationExists()
    {
        // Arrange
        const string initialSelectedKey = CalendarConfigurationKeys.Appointments.CreationOptions.RequireConfirmation;
        const string newSelectedKey = CalendarConfigurationKeys.Appointments.CreationOptions.Direct;
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(selectedKey: initialSelectedKey);
        calendar.AddConfiguration(configuration);

        // Act
        calendar.UpdateConfiguration(
            configurationId: configuration.Id,
            category: CalendarConfigurationKeys.Appointments.CreationStrategy,
            selectedKey: newSelectedKey);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarConfigurationUpdatedDomainEvent);
    }
}
