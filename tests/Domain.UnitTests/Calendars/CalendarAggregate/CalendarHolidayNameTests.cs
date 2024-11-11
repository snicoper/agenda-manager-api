using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarHolidayNameTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void CalendarHolidayName_ShouldRaiseException_WhenInvalidNameIsSet(int lengthName)
    {
        // Arrange
        var name = new string('*', lengthName);

        var calendarHoliday = () => CalendarHolidayFactory.CreateCalendarHoliday(name: name);

        // Assert
        calendarHoliday.Should().Throw<CalendarDomainException>();
        calendarHoliday.Should().Throw<CalendarDomainException>()
            .WithMessage("Name is invalid or exceeds length of 50 characters.");
    }
}
