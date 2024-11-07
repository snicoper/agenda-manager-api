using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Exxceptions;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services;

public class CalendarManagerTests
{
    private readonly CalendarManager _sut;
    private readonly ICalendarRepository _calendarRepository;

    public CalendarManagerTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _sut = new CalendarManager(_calendarRepository);
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenValidCalendarIsProvided()
    {
        // Act
        var result = await CreateCalendarAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value?.DomainEvents.Count.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Calendar_ShouldThrowCalendarDomainException_WhenInvalidNameIsProvided(int characters)
    {
        // Arrange
        var name = new string('a', characters);

        // Act & Assert
        await Assert.ThrowsAsync<CalendarDomainException>(
            () => _sut.CreateAsync(
                calendarId: CalendarId.Create(),
                name: name,
                description: "Description of my calendar",
                CancellationToken.None));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public async Task Calendar_ShouldThrowCalendarDomainException_WhenInvalidDescriptionIsProvided(int characters)
    {
        // Arrange
        var description = new string('a', characters);

        // Act & Assert
        await Assert.ThrowsAsync<CalendarDomainException>(
            () => _sut.CreateAsync(
                calendarId: CalendarId.Create(),
                name: "My calendar",
                description: description,
                CancellationToken.None));
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenNameNotAlreadyExists()
    {
        // Act
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);
        var result = await CreateCalendarAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Act
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await CreateCalendarAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Name));
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenDescriptionNotAlreadyExists()
    {
        // Act
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);
        var result = await CreateCalendarAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenDescriptionAlreadyExists()
    {
        // Act
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await CreateCalendarAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Description));
    }

    private async Task<Result<Calendar>> CreateCalendarAsync()
    {
        var calendar = CalendarFactory.CreateCalendar();

        var result = await _sut.CreateAsync(
            calendarId: calendar.Id,
            name: calendar.Name,
            description: calendar.Description,
            CancellationToken.None);

        return result;
    }
}
