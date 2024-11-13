using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarCreateTests
{
    private readonly Calendar _calendar = CalendarFactory.CreateCalendar();

    [Fact]
    public void Calendar_ShouldBeCreated_WhenValidParametersProvided()
    {
        // Assert
        _calendar.Should().NotBeNull();
        _calendar.Id.Should().Be(_calendar.Id);
        _calendar.Name.Should().Be(_calendar.Name);
        _calendar.Description.Should().Be(_calendar.Description);
    }

    [Fact]
    public void Calendar_ShouldBeActive_WhenCreated()
    {
        // Assert
        _calendar.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Calendar_ShouldRaiseEvent_WhenCreated()
    {
        // Assert
        _calendar.DomainEvents.Should().Contain(x => x is CalendarCreatedDomainEvent);
    }
}
