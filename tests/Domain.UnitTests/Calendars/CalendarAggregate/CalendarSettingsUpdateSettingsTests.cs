using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarSettingsUpdateSettingsTests
{
    [Theory]
    [InlineData(IanaTimeZoneConstants.AmericaNewYork)]
    [InlineData(IanaTimeZoneConstants.EuropeMadrid)]
    [InlineData(IanaTimeZoneConstants.Utc)]
    public void SettingsUpdate_ShouldUpdateSuccessfully_WhenValidParametersProvided(string newTimeZone)
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(IanaTimeZone.FromIana(newTimeZone), calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.Settings.IanaTimeZone.Value.Should().Be(newTimeZone);
    }

    [Fact]
    public void SettingsUpdate_ShouldRaiseEvent_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(
            IanaTimeZone.FromIana(IanaTimeZoneConstants.AmericaNewYork),
            calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void SettingsUpdate_ShouldNotRaiseEvent_WhenSameParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(calendar.Settings.IanaTimeZone, calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarSettingsUpdatedDomainEvent);
    }
}
