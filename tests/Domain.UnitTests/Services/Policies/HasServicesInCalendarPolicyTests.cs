using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Policies;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Services.Policies;

public class HasServicesInCalendarPolicyTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly HasServicesInCalendarPolicy _sut;

    public HasServicesInCalendarPolicyTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _sut = new HasServicesInCalendarPolicy(_serviceRepository);
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnTrue_WhenHasServicesInCalendar()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _serviceRepository.HasServicesInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnFalse_WhenHasNoServicesInCalendar()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _serviceRepository.HasServicesInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId);

        // Assert
        result.Should().BeFalse();
    }
}
