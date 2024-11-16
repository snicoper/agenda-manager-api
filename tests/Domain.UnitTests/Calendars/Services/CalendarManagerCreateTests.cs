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

public class CalendarManagerCreateTests
{
    private readonly CalendarManagerCreate _sut;
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarConfigurationOptionRepository _calendarConfigurationOptionRepository;

    public CalendarManagerCreateTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _calendarConfigurationOptionRepository = Substitute.For<ICalendarConfigurationOptionRepository>();

        _sut = new CalendarManagerCreate(_calendarRepository, _calendarConfigurationOptionRepository);
    }

    [Fact]
    public async Task Calendar_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        SetupNameExistsInCalendarRepositoryAsync(false);
        SetupGetAllInCalendarConfigurationOptionRepository(false);

        // Act
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsSuccess.Should().BeTrue();
        calendarResult.ResultType.Should().Be(ResultType.Created);
    }

    [Fact]
    public async Task Calendar_ShouldFail_WhenNameAlreadyExists()
    {
        // Arrange
        SetupNameExistsInCalendarRepositoryAsync(true);

        // Act
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsFailure.Should().BeTrue();
        calendarResult.Error?.FirstError().Should().Be(CalendarErrors.NameAlreadyExists.FirstError());
    }

    [Fact]
    public async Task Calendar_ShouldFail_WhenNoDefaultConfigurationsFound()
    {
        // Arrange
        SetupNameExistsInCalendarRepositoryAsync(false);
        SetupGetAllInCalendarConfigurationOptionRepository(true);

        // Act
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsFailure.Should().BeTrue();
        calendarResult.Error?.FirstError().Should()
            .Be(CalendarConfigurationOptionErrors.NoDefaultConfigurationsFound.FirstError());
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

    private void SetupNameExistsInCalendarRepositoryAsync(bool returnValue)
    {
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupGetAllInCalendarConfigurationOptionRepository(bool returnEmpty)
    {
        var returnValue = returnEmpty
            ? []
            : CalendarConfigurationOptionFactory.GetAll();

        _calendarConfigurationOptionRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
