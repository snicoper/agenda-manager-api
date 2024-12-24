using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManagerTests;

public class CalendarManagerUpdateTests : CalendarManagerTestsBase
{
    [Fact]
    public async Task Update_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupCalendarRepositoryGetByIdAsync(calendar);
        SetupExistsByNameInCalendarRepositoryAsync(false);

        // Act
        var result = await Sut.UpdateCalendarAsync(
            calendarId: calendar.Id,
            name: calendar.Name,
            description: calendar.Description,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldReturnFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupExistsByNameInCalendarRepositoryAsync(true);

        // Act
        var result = await Sut.UpdateCalendarAsync(
            calendarId: calendar.Id,
            name: calendar.Name,
            description: calendar.Description,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupCalendarRepositoryGetByIdAsync(Calendar returnValue)
    {
        CalendarRepository.GetByIdAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
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
