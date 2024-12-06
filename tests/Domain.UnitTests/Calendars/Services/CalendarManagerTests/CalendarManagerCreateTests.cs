using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManagerTests;

public class CalendarManagerCreateTests : CalendarManagerTestsBase
{
    [Fact]
    public async Task Create_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        SetupExistsByNameInCalendarRepositoryAsync(false);

        // Act
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsSuccess.Should().BeTrue();
        calendarResult.ResultType.Should().Be(ResultType.Created);
    }

    [Fact]
    public async Task Create_ShouldFailure_WhenNameAlreadyExists()
    {
        // Arrange
        SetupExistsByNameInCalendarRepositoryAsync(true);

        // Act
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsFailure.Should().BeTrue();
        calendarResult.Error?.FirstError().Should().Be(CalendarErrors.NameAlreadyExists.FirstError());
    }

    private async Task<Result<Calendar>> CreateCalendarAsync()
    {
        var calendar = CalendarFactory.CreateCalendar();

        var result = await Sut.CreateCalendarAsync(
            calendarId: calendar.Id,
            ianaTimeZone: IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
            name: calendar.Name,
            description: calendar.Description,
            cancellationToken: CancellationToken.None);

        return result;
    }

    private void SetupExistsByNameInCalendarRepositoryAsync(bool returnValue)
    {
        CalendarRepository.ExistsByNameAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
