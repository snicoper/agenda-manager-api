using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarUpdateTests
{
    private readonly Calendar _calendar = CalendarFactory.CreateCalendar();

    [Fact]
    public void Calendar_ShouldSucceed_WhenUpdatedWithValidData()
    {
        // Act
        _calendar.Update("New Name", "New Description");

        // Assert
        _calendar.Name.Should().Be("New Name");
        _calendar.Description.Should().Be("New Description");
    }

    [Fact]
    public void Calendar_ShouldRaiseEvent_WhenUpdatedWithValidData()
    {
        // Act
        _calendar.Update("New Name", "New Description");

        // Assert
        _calendar.DomainEvents.Should().Contain(x => x is CalendarUpdatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Calendar_ShouldThrowException_WhenNameIsInvalid(int newNameLength)
    {
        // Arrange
        var newName = new string('a', newNameLength);

        // Act
        var action = () => _calendar.Update(newName, "New Description");

        // Assert
        action.Should().Throw<CalendarDomainException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Calendar_ShouldThrowException_WhenDescriptionIsInvalid(int newDescriptionLength)
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
