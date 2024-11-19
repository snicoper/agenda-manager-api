using AgendaManager.Domain.Common.Utils;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Utils;

public class TimeZoneUtilsTests
{
    [Theory]
    [InlineData(IanaTimeZoneConstants.EuropeMadrid)]
    [InlineData(IanaTimeZoneConstants.AmericaNewYork)]
    [InlineData(IanaTimeZoneConstants.Utc)]
    public void GetTimeZoneInfoFromIana_ShouldWork_WithValidIds(string ianaId)
    {
        // Act
        var result = TimeZoneUtils.GetTimeZoneInfoFromIana(ianaId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Theory]
    [InlineData(IanaTimeZoneConstants.InvalidTimeZone)]
    [InlineData(IanaTimeZoneConstants.NotATimeZone)]
    public void GetTimeZoneInfoFromIana_ShouldFailure_WithInvalidIds(string invalidId)
    {
        // Act
        var result = TimeZoneUtils.GetTimeZoneInfoFromIana(invalidId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void Cache_ShouldReturnSameInstance()
    {
        // Arrange
        TimeZoneUtils.ClearCache();
        const string ianaId = IanaTimeZoneConstants.EuropeMadrid;

        // Act
        var result1 = TimeZoneUtils.GetTimeZoneInfoFromIana(ianaId);
        var result2 = TimeZoneUtils.GetTimeZoneInfoFromIana(ianaId);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result1.Value.Should().BeSameAs(result2.Value);
    }

    [Fact]
    public void Methods_ShouldHandleNullInput()
    {
        string? nullValue = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () =>
                TimeZoneUtils.GetTimeZoneInfoFromIana(nullValue!));

        Assert.Throws<ArgumentNullException>(
            () =>
                TimeZoneUtils.TryConvertWindowsIdToIanaId(nullValue!));

        Assert.Throws<ArgumentNullException>(
            () =>
                TimeZoneUtils.IsValidIanaTimeZone(nullValue!));
    }
}
