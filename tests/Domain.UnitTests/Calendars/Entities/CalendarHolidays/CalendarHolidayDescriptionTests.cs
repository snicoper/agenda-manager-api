using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayDescriptionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void CalendarHoliday_ShouldThrowException_WhenDescriptionIsInvalid(int descriptionLength)
    {
        // Arrange
        var description = new string('*', descriptionLength);

        // Act
        var action = () => CalendarHolidayFactory.CreateCalendarHoliday(description: description);

        // Assert
        action.Should().Throw<CalendarHolidayDomainException>();
        action.Should().Throw<CalendarHolidayDomainException>()
            .WithMessage("Description is invalid or exceeds length of 500 characters.");
    }
}
