using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars;

public class CalendarTests
{
    [Fact]
    public void Calendar_ShouldReturnCalendar_WhenCalendarIsCreated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act & Assert
        calendar.Should().NotBeNull();
        calendar.Id.Should().Be(calendar.Id);
        calendar.Name.Should().Be(calendar.Name);
        calendar.Description.Should().Be(calendar.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Calendar_ShouldThrowDomainException_WhenInvalidNameIsProvided(int characters)
    {
        // Arrange
        var name = new string('a', characters);

        // Act
        var exception = Assert.Throws<DomainException>(() => CalendarFactory.CreateCalendar(name: name));

        // Assert
        exception.Message.Should().Be("Name cannot be empty and must be less than 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Calendar_ShouldThrowDomainException_WhenInvalidDescriptionIsProvided(int characters)
    {
        // Arrange
        var description = new string('a', characters);

        // Act
        var exception = Assert.Throws<DomainException>(() => CalendarFactory.CreateCalendar(description: description));

        // Assert
        exception.Message.Should().Be("Description cannot be empty and must be less than 500 characters.");
    }
}
