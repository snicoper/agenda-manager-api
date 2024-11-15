using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Services.Services.ServiceManagers;

public class ServiceManagerCreateTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ICalendarRepository _calendarRepository;
    private readonly ServiceManager _sut;

    public ServiceManagerCreateTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _calendarRepository = Substitute.For<ICalendarRepository>();

        _sut = new ServiceManager(_serviceRepository, _calendarRepository);
    }

    [Fact]
    public async Task Create_ShouldCreated_WhenValidDataIsPassed()
    {
        // Arrange
        var serviceManager = ServiceFactory.CreateService();
        SetupCalendarIdExistsCalendarRepository(false);
        SetupNameExistsServiceRepository(false);

        // Act
        var result = await _sut.CreateServiceAsync(
            serviceManager.Id,
            serviceManager.CalendarId,
            serviceManager.Duration,
            serviceManager.Name,
            serviceManager.Description,
            serviceManager.ColorScheme,
            serviceManager.IsActive);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldFail_WhenCalendarIdDoesNotExist()
    {
        // Arrange
        var serviceManager = ServiceFactory.CreateService();
        SetupCalendarIdExistsCalendarRepository(true);

        // Act
        var result = await _sut.CreateServiceAsync(
            serviceManager.Id,
            serviceManager.CalendarId,
            serviceManager.Duration,
            serviceManager.Name,
            serviceManager.Description,
            serviceManager.ColorScheme,
            serviceManager.IsActive);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarErrors.CalendarNotFound.FirstError());
    }

    [Fact]
    public async Task Create_ShouldFail_WhenNameAlreadyExists()
    {
        // Arrange
        var serviceManager = ServiceFactory.CreateService();
        SetupCalendarIdExistsCalendarRepository(false);
        SetupNameExistsServiceRepository(true);

        // Act
        var result = await _sut.CreateServiceAsync(
            serviceManager.Id,
            serviceManager.CalendarId,
            serviceManager.Duration,
            serviceManager.Name,
            serviceManager.Description,
            serviceManager.ColorScheme,
            serviceManager.IsActive);

        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.NameAlreadyExists.FirstError());
    }

    private void SetupCalendarIdExistsCalendarRepository(bool returnValue)
    {
        _calendarRepository.CalendarIdExistsAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupNameExistsServiceRepository(bool returnValue)
    {
        _serviceRepository.NameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
