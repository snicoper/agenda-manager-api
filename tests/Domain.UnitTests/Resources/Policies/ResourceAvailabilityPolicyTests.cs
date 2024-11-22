using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.Policies;
using AgendaManager.TestCommon.Factories;
using AgendaManager.TestCommon.Factories.ValueObjects;
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
    public async Task IsAvailableAsync_ShouldSuccess_WhenAllResourcesAreAvailable()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var calendarConfiguration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.ResourcesSchedules.ResourcesScheduleValidationStrategy,
            selectedKey: CalendarConfigurationKeys.ResourcesSchedules.SchedulesValidationOptions.NotValidate);

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
            [calendarConfiguration],
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldFailure_WhenConfigurationIsNotDefined()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var calendarConfiguration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        var calendarId = CalendarId.Create();
        var period = PeriodFactory.Create();

        // Act
        var result = await _sut.IsAvailableAsync(
            calendarId,
            resources,
            period,
            [calendarConfiguration],
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarConfigurationErrors.KeyNotFound.FirstError());
    }

    [Fact]
    public async Task IsAvailableAsync_ShouldFailure_WhenAllResourcesAreNotAvailable()
    {
        // Arrange
        List<Resource> resources = [ResourceFactory.CreateResource()];
        var calendarConfiguration = CalendarConfigurationFactory.CreateCalendarConfiguration();
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
            [calendarConfiguration],
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
