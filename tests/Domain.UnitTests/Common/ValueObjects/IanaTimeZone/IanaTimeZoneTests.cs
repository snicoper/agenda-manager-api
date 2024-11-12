using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.IanaTimeZone;

public class IanaTimeZoneTests
{
    [Theory]
    [InlineData(IanaTimeZoneConstants.UTC)]
    [InlineData(IanaTimeZoneConstants.AsiaTokyo)]
    [InlineData(IanaTimeZoneConstants.EuropeMadrid)]
    [InlineData(IanaTimeZoneConstants.AmericaNewYork)]
    public void IanaTimeZone_FromIana_ShouldCreate_WhenValidTimeZone(string validIanaId)
    {
        // Act
        var timeZone = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(validIanaId);

        // Assert
        timeZone.Value.Should().Be(validIanaId);
        timeZone.Info.Should().NotBeNull();
    }

    [Fact]
    public void IanaTimeZone_FromIana_ShouldThrowException_WhenValueIsNull()
    {
        // Act
        var action = () => Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(IanaTimeZoneConstants.InvalidTimeZone)]
    public void IanaTimeZone_FromIana_ShouldThrowException_WhenValueIsInvalid(string invalidTimeZone)
    {
        // Act
        var action = () => Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(invalidTimeZone);

        // Assert
        action.Should().Throw<CalendarSettingsException>();
    }

    [Fact]
    public void IanaTimeZone_FromTimeZoneInfo_ShouldThrowException_WhenValueIsNull()
    {
        // Act
        var action = () => Domain.Calendars.ValueObjects.IanaTimeZone.FromTimeZoneInfo(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IanaTimeZone_FromTimeZoneInfo_ShouldCreateValidTimeZone_FromSystemTimeZone()
    {
        // Arrange
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? IanaTimeZoneConstants.ValidWindows : IanaTimeZoneConstants.EuropeMadrid);

        // Act
        var timeZone = Domain.Calendars.ValueObjects.IanaTimeZone.FromTimeZoneInfo(timeZoneInfo);

        // Assert
        timeZone.Should().NotBeNull();
        timeZone.Info.Should().NotBeNull();
    }

    [Fact]
    public void IanaTimeZone_ShouldBeEqual_WhenEqualTimeZones()
    {
        // Arrange
        var timeZone1 = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);
        var timeZone2 = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);

        // Assert
        timeZone1.Should().Be(timeZone2);
        timeZone1.GetHashCode().Should().Be(timeZone2.GetHashCode());
    }

    [Fact]
    public void IanaTimeZone_ShouldNotBeEqual_WhenDifferentTimeZones()
    {
        // Arrange
        var timeZone1 = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);
        var timeZone2 = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeLondon);

        // Assert
        timeZone1.Should().NotBe(timeZone2);
        timeZone1.GetHashCode().Should().NotBe(timeZone2.GetHashCode());
    }

    [Fact]
    public void IanaTimeZone_Should_HandleUTCCorrectly()
    {
        // Arrange & Act
        var timeZone = Domain.Calendars.ValueObjects.IanaTimeZone.FromIana(IanaTimeZoneConstants.UTC);

        // Assert
        timeZone.Value.Should().Be(IanaTimeZoneConstants.UTC);
        timeZone.Info.BaseUtcOffset.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void IanaTimeZone_FromTimeZoneInfo_ShouldConvertWindows_ToIanaCorrectly()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        // Arrange
        var windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(IanaTimeZoneConstants.ValidWindows);

        // Act
        var timeZone = Domain.Calendars.ValueObjects.IanaTimeZone.FromTimeZoneInfo(windowsTimeZone);

        // Assert
        timeZone.Should().NotBeNull();
        timeZone.Value.Should().NotBe(IanaTimeZoneConstants.ValidWindows);
        timeZone.Info.Should().NotBeNull();
    }
}
