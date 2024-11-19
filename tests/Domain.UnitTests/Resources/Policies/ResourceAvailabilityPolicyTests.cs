using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.Policies;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Resources.Policies;

public class ResourceAvailabilityPolicyTests
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ResourceAvailabilityPolicy _sut;

    public ResourceAvailabilityPolicyTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        _sut = new ResourceAvailabilityPolicy(_resourceRepository);
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnSuccess_WhenAllResourcesAreAvailable()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var calendarId = CalendarId.Create();
        var period = PeriodFactory.Create();

        _resourceRepository.AreResourcesAvailableInPeriodAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.IsAvailableAsync(
            calendarId,
            resources,
            period,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldReturnFailure_WhenAllResourcesAreNotAvailable()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var calendarId = CalendarId.Create();
        var period = PeriodFactory.Create();

        _resourceRepository.AreResourcesAvailableInPeriodAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsAvailableAsync(
            calendarId,
            resources,
            period,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
