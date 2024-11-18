using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Policies;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Policies;

public class CalendarHolidayAvailabilityPolicyTests
{
    private readonly ICalendarHolidayRepository _holidayRepository;
    private readonly CalendarHolidayAvailabilityPolicy _sut;

    public CalendarHolidayAvailabilityPolicyTests()
    {
        _holidayRepository = Substitute.For<ICalendarHolidayRepository>();
        _sut = new CalendarHolidayAvailabilityPolicy(_holidayRepository);
    }

    [Fact]
    public async Task IsAvailable_ShouldReturnSuccess_WhenThereIsNoOverlapping()
    {
        // Arrange
        _holidayRepository
            .IsOverlappingInPeriodByCalendarIdAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        // Act
        var result = await _sut.IsAvailableAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailable_ShouldReturnFailure_WhenThereIsOverlapping()
    {
        // Arrange
        _holidayRepository
            .IsOverlappingInPeriodByCalendarIdAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(CalendarHolidayErrors.HolidaysOverlap);

        // Act
        var result = await _sut.IsAvailableAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should()
            .Be(CalendarHolidayErrors.HolidaysOverlap.FirstError());
    }
}
