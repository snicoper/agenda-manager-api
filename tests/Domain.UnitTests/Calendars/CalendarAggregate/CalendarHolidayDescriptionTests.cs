﻿using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarHolidayDescriptionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void CalendarHolidayDescription_ShouldRaiseException_WhenInvalidDescriptionIsSet(int lengthDescription)
    {
        // Arrange
        var description = new string('*', lengthDescription);

        // Act
        var calendarHoliday = () => CalendarHolidayFactory.CreateCalendarHoliday(description: description);

        // Assert
        calendarHoliday.Should().Throw<CalendarDomainException>();
        calendarHoliday.Should().Throw<CalendarDomainException>()
            .WithMessage("Description is invalid or exceeds length of 500 characters.");
    }
}
