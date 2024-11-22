using AgendaManager.Domain.Calendars;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManager;

public class CalendarManagerUpdateTests : CalendarManagerBase
{
    [Fact]
    public async Task Update_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupExistsByNameInCalendarRepositoryAsync(false);

        // Act
        var result = await Sut.UpdateCalendarAsync(calendar, CancellationToken.None);

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
        var result = await Sut.UpdateCalendarAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupExistsByNameInCalendarRepositoryAsync(bool returnValue)
    {
        CalendarRepository.ExistsByNameAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
