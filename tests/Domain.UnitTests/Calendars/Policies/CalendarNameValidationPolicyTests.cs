using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Policies;
using AgendaManager.Domain.Calendars.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Policies;

public class CalendarNameValidationPolicyTests
{
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarNameValidationPolicy _sut;

    public CalendarNameValidationPolicyTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _sut = new CalendarNameValidationPolicy(_calendarRepository);
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnTrue_WhenCalendarExists()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        const string name = "Test Calendar";

        _calendarRepository.ExistsByNameAsync(calendarId, name, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.ExistsAsync(calendarId, name);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnFalse_WhenCalendarDoesNotExist()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        const string name = "Test Calendar";

        _calendarRepository.ExistsByNameAsync(calendarId, name, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.ExistsAsync(calendarId, name);

        // Assert
        result.Should().BeFalse();
    }
}
