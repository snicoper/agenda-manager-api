using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayNameTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void CalendarHolidayName_ShouldRaiseException_WhenInvalidNameIsSet(int nameLength)
    {
        // Arrange
        var name = new string('*', nameLength);

        // Act
        var calendarHoliday = () => CalendarHolidayFactory.CreateCalendarHoliday(name: name);

        // Assert
        calendarHoliday.Should().Throw<CalendarHolidayDomainException>();
        calendarHoliday.Should().Throw<CalendarHolidayDomainException>()
            .WithMessage("Name is invalid or exceeds length of 50 characters.");
    }
}
