using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Policies;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.Policies;

public class HasResourcesInCalendarPolicyTests
{
    private readonly IResourceRepository _resourceRepository;
    private readonly HasResourcesInCalendarPolicy _sut;

    public HasResourcesInCalendarPolicyTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        _sut = new HasResourcesInCalendarPolicy(_resourceRepository);
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnTrue_WhenHasResourcesInCalendar()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _resourceRepository.HasResourcesInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnFalse_WhenHasNoResourcesInCalendar()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _resourceRepository.HasResourcesInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId);

        // Assert
        result.Should().BeFalse();
    }
}
