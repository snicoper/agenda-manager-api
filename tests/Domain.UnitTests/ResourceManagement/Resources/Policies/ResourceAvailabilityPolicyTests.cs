using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Policies;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.Policies;

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
    public async Task IsAvailable_ShouldSuccess_WhenResourceSchedulesAvailabilityTypeAreValidate()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var settings = CalendarSettingsFactory.CreateCalendarSettings(
            resourceSchedulesAvailability: ResourceScheduleValidationStrategy.Validate);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        var period = PeriodFactory.Create();

        _resourceRepository.AreResourcesAvailableInPeriodAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _sut.IsAvailableAsync(
            calendar: calendar,
            resources: resources,
            period: period,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldSuccess_WhenResourceSchedulesAvailabilityTypeAreIgnore()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var settings = CalendarSettingsFactory.CreateCalendarSettings(
            resourceSchedulesAvailability: ResourceScheduleValidationStrategy.Ignore);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        var period = PeriodFactory.Create();

        // Act
        var result = await _sut.IsAvailableAsync(
            calendar: calendar,
            resources: resources,
            period: period,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldFailure_WhenAllResourcesAreNotAvailable()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var settings = CalendarSettingsFactory.CreateCalendarSettings(
            resourceSchedulesAvailability: ResourceScheduleValidationStrategy.Validate);
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        var period = PeriodFactory.Create();

        _resourceRepository.AreResourcesAvailableInPeriodAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<List<Resource>>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsAvailableAsync(
            calendar: calendar,
            resources: resources,
            period: period,
            cancellationToken: CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
