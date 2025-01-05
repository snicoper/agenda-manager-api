using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarHolidayManagerTests;

public class UpdateHolidayTests : CalendarHolidayManagerTestsBase
{
    [Fact]
    public async Task UpdateHoliday_ShouldReturnHoliday_WhenValidValuesAreProvided()
    {
        // Arrange
        var calendarHoliday = CalendarHolidayFactory.CreateCalendarHoliday();
        var calendar = CalendarFactory.CreateCalendar();
        calendar.AddHoliday(calendarHoliday);

        SetupCalendarRepositoryGetByIdWithHolidays(calendar);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(exists: false);
        SetupCalendarHolidayAvailabilityExcludeSelfPolicyIsAvailable(overlaps: false);

        // Act
        var result = await SutUpdateHolidayAsync(calendar);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateHoliday_ShouldReturnNameAlreadyExists_WhenHolidayNameAlreadyExists()
    {
        // Arrange
        var calendarHoliday = CalendarHolidayFactory.CreateCalendarHoliday();
        var calendar = CalendarFactory.CreateCalendar();
        calendar.AddHoliday(calendarHoliday);

        SetupCalendarRepositoryGetByIdWithHolidays(calendar);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(exists: true);

        // Act
        var result = await SutUpdateHolidayAsync(calendar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.NameAlreadyExists.FirstError());
    }

    [Fact]
    public async Task UpdateHoliday_ShouldReturnHolidaysOverlap_WhenHolidayOverlaps()
    {
        // Arrange
        var calendarHoliday = CalendarHolidayFactory.CreateCalendarHoliday();
        var calendar = CalendarFactory.CreateCalendar();
        calendar.AddHoliday(calendarHoliday);

        SetupCalendarRepositoryGetByIdWithHolidays(calendar);
        SetupCalendarHolidayRepositoryExistsHolidayNameAsync(exists: false);
        SetupCalendarHolidayAvailabilityExcludeSelfPolicyIsAvailable(overlaps: true);

        // Act
        var result = await SutUpdateHolidayAsync(calendar);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.FirstError().Should().Be(CalendarHolidayErrors.HolidaysOverlap.FirstError());
    }

    private async Task<Result> SutUpdateHolidayAsync(Calendar calendar)
    {
        var calendarHoliday = calendar.Holidays[0];
        const string newName = "New Holiday";
        var period = PeriodFactory.Create(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);

        return await Sut.UpdateHolidayAsync(
            calendarId: calendar.Id,
            calendarHolidayId: calendarHoliday.Id,
            name: newName,
            period: period,
            cancellationToken: CancellationToken.None);
    }

    private void SetupCalendarRepositoryGetByIdWithHolidays(Calendar? calendarResult = null)
    {
        CalendarRepository.GetByIdWithHolidaysAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(calendarResult);
    }

    private void SetupCalendarHolidayRepositoryExistsHolidayNameAsync(bool exists)
    {
        CalendarHolidayRepository.ExistsHolidayNameAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<CalendarHolidayId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(exists);
    }

    private void SetupCalendarHolidayAvailabilityExcludeSelfPolicyIsAvailable(bool overlaps)
    {
        CalendarHolidayAvailabilityExcludeSelfPolicy.IsAvailableAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<CalendarHolidayId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(overlaps ? CalendarHolidayErrors.HolidaysOverlap : Result.Success());
    }
}
