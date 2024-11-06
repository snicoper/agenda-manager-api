using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services;

public class CalendarServiceTests
{
    private readonly CalendarService _sut;
    private readonly ICalendarRepository _calendarRepository;

    public CalendarServiceTests()
    {
        _calendarRepository = Substitute.For<ICalendarRepository>();
        _sut = new CalendarService(_calendarRepository);
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenValidCalendarIsProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public async Task Calendar_ShouldReturnResultFailure_WhenInvalidNameIsProvided(int characters)
    {
        // Arrange
        var name = new string('a', characters);
        var calendar = CalendarFactory.CreateCalendar(name: name);

        // Act
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Name));
        result.Error?.FirstError()?.Description.Should()
            .Be("Name cannot be empty and must be less than 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public async Task Calendar_ShouldReturnResultFailure_WhenInvalidDescriptionIsProvided(int characters)
    {
        // Arrange
        var description = new string('a', characters);
        var calendar = CalendarFactory.CreateCalendar(description: description);

        // Act
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Description));
        result.Error?.FirstError()?.Description.Should()
            .Be("Description cannot be empty and must be less than 500 characters.");
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenNameNotAlreadyExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        _calendarRepository.NameExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Name));
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultSuccess_WhenDescriptionNotAlreadyExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(false);
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Calendar_ShouldReturnResultFailure_WhenDescriptionAlreadyExists()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        _calendarRepository.DescriptionExistsAsync(Arg.Any<Calendar>(), Arg.Any<CancellationToken>()).Returns(true);
        var result = await _sut.CreateAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Code.Should().Be(nameof(Calendar.Description));
    }
}
