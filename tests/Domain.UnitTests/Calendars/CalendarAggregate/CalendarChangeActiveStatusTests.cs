using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarChangeActiveStatusTests
{
    private readonly Calendar _calendar = CalendarFactory.CreateCalendar();

    [Fact]
    public void ChangeActiveStatus_ShouldChangeActiveStatus_WhenActiveStatusIsChanged()
    {
        // Arrange
        var newActiveStatus = !_calendar.IsActive;

        // Act
        _calendar.ChangeActiveStatus(newActiveStatus);

        // Assert
        _calendar.IsActive.Should().Be(newActiveStatus);
    }

    [Fact]
    public void ChangeActiveStatus_ShouldRaiseDomainEvent_WhenActiveStatusIsChanged()
    {
        // Arrange
        var newActiveStatus = !_calendar.IsActive;

        // Act
        _calendar.ChangeActiveStatus(newActiveStatus);

        // Assert
        _calendar.DomainEvents.Should().Contain(x => x is CalendarActiveStatusChangedDomainEvent);
    }
}
