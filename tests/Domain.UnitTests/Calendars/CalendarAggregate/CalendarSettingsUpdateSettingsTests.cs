using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarSettingsUpdateSettingsTests
{
    [Theory]
    [InlineData(TimeZoneConstants.AmericaNewYork)]
    [InlineData(TimeZoneConstants.EuropeMadrid)]
    [InlineData(TimeZoneConstants.UTC)]
    public void Update_ShouldSuccessfully_WhenUpdateSettings(string newTimeZone)
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(newTimeZone, calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.Settings.TimeZone.Should().Be(newTimeZone);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenUpdateSettingsWithInvalidTimeZone()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const string invalidTimeZone = "InvalidTimeZone";

        // Act
        var action = () => calendar.UpdateSettings(invalidTimeZone, calendar.Settings.HolidayCreationStrategy);

        // Assert
        action.Should().Throw<CalendarSettingsException>()
            .WithMessage("Invalid IANA time zone ID.");
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenUpdateSettingsWithValidTimeZone()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const string newTimeZone = TimeZoneConstants.AmericaNewYork;

        // Act
        calendar.UpdateSettings(newTimeZone, calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenUpdateSettingsWithSameValues()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(calendar.Settings.TimeZone, calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarSettingsUpdatedDomainEvent);
    }
}
