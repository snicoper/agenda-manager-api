using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarChangeActiveStatusTests
{
    [Fact]
    public void ChangeActiveStatus_ShouldChangeActiveStatus_WhenActiveStatusIsChanged()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var newActiveStatus = !calendar.IsActive;

        // Act
        calendar.ChangeActiveStatus(newActiveStatus);

        // Assert
        calendar.IsActive.Should().Be(newActiveStatus);
    }

    [Fact]
    public void ChangeActiveStatus_ShouldRaiseDomainEvent_WhenActiveStatusIsChanged()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var newActiveStatus = !calendar.IsActive;

        // Act
        calendar.ChangeActiveStatus(newActiveStatus);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarActiveStatusChangedDomainEvent);
    }
}
