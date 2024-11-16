using AgendaManager.Domain.Common.ValueObjects.IanaTimeZone;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.IanaTimeZoneTests;

public class IanaTimeZoneTests
{
    [Theory]
    [InlineData(IanaTimeZoneConstants.Utc)]
    [InlineData(IanaTimeZoneConstants.AsiaTokyo)]
    [InlineData(IanaTimeZoneConstants.EuropeMadrid)]
    [InlineData(IanaTimeZoneConstants.AmericaNewYork)]
    public void IanaTimeZone_FromIana_ShouldCreate_WhenValidTimeZone(string validIanaId)
    {
        // Act
        var timeZone = IanaTimeZone.FromIana(validIanaId);

        // Assert
        timeZone.Value.Should().Be(validIanaId);
        timeZone.Info.Should().NotBeNull();
    }

    [Fact]
    public void IanaTimeZone_FromIana_ShouldThrowException_WhenValueIsNull()
    {
        // Act
        var action = () => IanaTimeZone.FromIana(null!);

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
        var action = () => IanaTimeZone.FromIana(invalidTimeZone);

        // Assert
        action.Should().Throw<IanaTimeZoneDomainException>();
    }

    [Fact]
    public void IanaTimeZone_FromTimeZoneInfo_ShouldThrowException_WhenValueIsNull()
    {
        // Act
        var action = () => IanaTimeZone.FromTimeZoneInfo(null!);

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
        var timeZone = IanaTimeZone.FromTimeZoneInfo(timeZoneInfo);

        // Assert
        timeZone.Should().NotBeNull();
        timeZone.Info.Should().NotBeNull();
    }

    [Fact]
    public void IanaTimeZone_ShouldBeEqual_WhenEqualTimeZones()
    {
        // Arrange
        var timeZone1 = IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);
        var timeZone2 = IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);

        // Assert
        timeZone1.Should().Be(timeZone2);
        timeZone1.GetHashCode().Should().Be(timeZone2.GetHashCode());
    }

    [Fact]
    public void IanaTimeZone_ShouldNotBeEqual_WhenDifferentTimeZones()
    {
        // Arrange
        var timeZone1 = IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid);
        var timeZone2 = IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeLondon);

        // Assert
        timeZone1.Should().NotBe(timeZone2);
        timeZone1.GetHashCode().Should().NotBe(timeZone2.GetHashCode());
    }

    [Fact]
    public void IanaTimeZone_Should_HandleUtcCorrectly()
    {
        // Arrange & Act
        var timeZone = IanaTimeZone.FromIana(IanaTimeZoneConstants.Utc);

        // Assert
        timeZone.Value.Should().Be(IanaTimeZoneConstants.Utc);
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
        var timeZone = IanaTimeZone.FromTimeZoneInfo(windowsTimeZone);

        // Assert
        timeZone.Should().NotBeNull();
        timeZone.Value.Should().NotBe(IanaTimeZoneConstants.ValidWindows);
        timeZone.Info.Should().NotBeNull();
    }
}
