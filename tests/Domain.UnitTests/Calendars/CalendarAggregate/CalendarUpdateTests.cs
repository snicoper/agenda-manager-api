using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarUpdateTests
{
    [Fact]
    public void Calendar_ShouldUpdateCalendar_WhenIsValid()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.Update("New Name", "New Description");

        // Assert
        calendar.Name.Should().Be("New Name");
        calendar.Description.Should().Be("New Description");
    }

    [Fact]
    public void Calendar_ShouldRaiseEvent_WhenIsValid()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.Update("New Name", "New Description");

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarUpdatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Calendar_ShouldThrowException_WhenCalendarIsUpdatedWithInvalidName(int newNameLength)
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var newName = new string('a', newNameLength);

        // Act
        var action = () => calendar.Update(newName, "New Description");

        // Assert
        action.Should().Throw<CalendarDomainException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Calendar_ShouldThrowException_WhenCalendarIsUpdatedWithInvalidDescription(int newDescriptionLength)
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var newDescription = new string('a', newDescriptionLength);

        // Act
        var action = () => calendar.Update("New Name", newDescription);

        // Assert
        action.Should().Throw<CalendarDomainException>();
    }
}
