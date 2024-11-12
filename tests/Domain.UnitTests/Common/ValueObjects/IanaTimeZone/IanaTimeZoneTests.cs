using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.IanaTimeZone;

public class IanaTimeZoneTests
{
    [Fact]
    public void IanaTimeZone_Should_BeValid()
    {
        // Arrange
        var ianaTimeZone = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);

        // Act
        ianaTimeZone.Value.Should().Be(IanaTimeZoneConstants.EuropeMadrid);
    }

    [Fact]
    public void IanaTimeZone_ShouldRaiseException_BeInValid()
    {
        // Arrange
        var ianaTimeZone = () => Domain.Calendars.ValueObjects.IanaTimeZone.FromIana("invalid");

        // Act
        ianaTimeZone.Should().Throw<CalendarSettingsException>()
            .WithMessage("Invalid IANA time zone ID.");
    }
}
