using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exxceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarCreateTests
{
    [Fact]
    public void Calendar_ShouldReturnCalendar_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.Should().NotBeNull();
        calendar.Id.Should().Be(calendar.Id);
        calendar.Name.Should().Be(calendar.Name);
        calendar.Description.Should().Be(calendar.Description);
    }

    [Fact]
    public void Calendar_ShouldActiveTrue_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Calendar_ShouldRaiseDomainEvent_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarCreatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Calendar_ShouldThrowException_WhenCalendarIsCreatedWithInvalidName(int nameLength)
    {
        // Arrange
        var name = new string('a', nameLength);

        // Act
        var calendar = () => CalendarFactory.CreateCalendar(name: name);

        // Assert
        calendar.Should().Throw<CalendarDomainException>();
        calendar.Should().Throw<CalendarDomainException>()
            .WithMessage("Name is invalid or exceeds length of 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Calendar_ShouldThrowException_WhenCalendarIsCreatedWithInvalidDescription(int descriptionLength)
    {
        // Arrange
        var description = new string('a', descriptionLength);

        // Act
        var calendar = () => CalendarFactory.CreateCalendar(description: description);

        // Assert
        calendar.Should().Throw<CalendarDomainException>();
        calendar.Should().Throw<CalendarDomainException>()
            .WithMessage("Description is invalid or exceeds length of 500 characters.");
    }
}
