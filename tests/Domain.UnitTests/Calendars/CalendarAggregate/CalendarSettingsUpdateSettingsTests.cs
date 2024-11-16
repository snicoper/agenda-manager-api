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
        var configuration = CalendarSettingsConfigurationFactory.CreateConfiguration(
            ianaTimeZone: IanaTimeZone.FromIana(newTimeZone));

        // Act
        calendar.UpdateSettings(configuration);

        // Assert
        calendar.Settings.IanaTimeZone.Value.Should().Be(newTimeZone);
    }

    [Fact]
    public void SettingsUpdate_ShouldRaiseEvent_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarSettingsConfigurationFactory.CreateConfiguration();

        // Act
        calendar.UpdateSettings(configuration);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void SettingsUpdate_ShouldNotRaiseEvent_WhenSameParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var configuration = CalendarSettingsConfigurationFactory.CreateConfiguration(
            ianaTimeZone: calendar.Settings.IanaTimeZone,
            holidayCreateStrategy: calendar.Settings.HolidayCreateStrategy,
            appointmentOverlappingStrategy: calendar.Settings.AppointmentOverlappingStrategy);

        // Act
        calendar.UpdateSettings(configuration);

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarSettingsUpdatedDomainEvent);
    }
}
