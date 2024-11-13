using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Constants;
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
        // Arrange
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsSuccess.Should().BeTrue();
        calendarResult.Value?.DomainEvents.Count.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Calendar_ShouldThrowCalendarDomainException_WhenInvalidNameIsProvided(int characters)
    {
        // Arrange
        var name = new string('a', characters);

        // Assert
        await Assert.ThrowsAsync<CalendarDomainException>(
            () => _sut.CreateCalendarAsync(
                calendarId: CalendarId.Create(),
                ianaTimeZone: IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
                name: name,
                description: "Description of my calendar",
                HolidayCreationStrategy.CancelOverlapping,
                cancellationToken: CancellationToken.None));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public async Task Calendar_ShouldThrowCalendarDomainException_WhenInvalidDescriptionIsProvided(int characters)
    {
        // Arrange
        var description = new string('a', characters);

        // Assert
        await Assert.ThrowsAsync<CalendarDomainException>(
            () => _sut.CreateCalendarAsync(
                calendarId: CalendarId.Create(),
                ianaTimeZone: IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
                name: "My calendar",
                description: description,
                HolidayCreationStrategy.CancelOverlapping,
                cancellationToken: CancellationToken.None));
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenNameNotAlreadyExists()
    {
        // Arrange
        var calendarResult = await CreateCalendarAsync();
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);

        // Assert
        calendarResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Arrange
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsFailure.Should().BeTrue();
        calendarResult.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Name));
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenDescriptionNotAlreadyExists()
    {
        // Arrange
        var calendarResult = await CreateCalendarAsync();
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);

        // Assert
        calendarResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenDescriptionAlreadyExists()
    {
        // Arrange
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var calendarResult = await CreateCalendarAsync();

        // Assert
        calendarResult.IsFailure.Should().BeTrue();
        calendarResult.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Description));
    }

    private async Task<Result<Calendar>> CreateCalendarAsync()
    {
        var calendar = CalendarFactory.CreateCalendar();

        var result = await _sut.CreateCalendarAsync(
            calendarId: calendar.Id,
            ianaTimeZone: IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
            name: calendar.Name,
            description: calendar.Description,
            HolidayCreationStrategy.CancelOverlapping,
            cancellationToken: CancellationToken.None);

        return result;
    }
}
