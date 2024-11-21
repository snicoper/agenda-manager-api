using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.IanaTimeZone;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services;

public class CalendarManagerTests
{
    private readonly CalendarManager _sut;
    private readonly ICalendarRepository _calendarRepository;

    public CalendarManagerTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();

        _sut = new CalendarManager(_calendarRepository);
    }

    [Fact]
    public async Task Calendar_ShouldReturnSucceed_WhenValidParametersProvided()
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
    public async Task Calendar_ShouldFailure_WhenNameAlreadyExists()
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

        var result = await _sut.CreateCalendarAsync(
            calendarId: calendar.Id,
            ianaTimeZone: IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
            name: calendar.Name,
            description: calendar.Description,
            cancellationToken: CancellationToken.None);

        return result;
    }

    private void SetupExistsByNameInCalendarRepositoryAsync(bool returnValue)
    {
        _calendarRepository.ExistsByNameAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
