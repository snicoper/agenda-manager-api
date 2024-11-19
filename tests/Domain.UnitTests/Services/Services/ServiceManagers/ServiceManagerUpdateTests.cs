using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Services;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Services.Services.ServiceManagers;

public class ServiceManagerUpdateTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ServiceManager _sut;

    public ServiceManagerUpdateTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        var calendarRepository = Substitute.For<ICalendarRepository>();
        var appointmentRepository = Substitute.For<IAppointmentRepository>();

        _sut = new ServiceManager(_serviceRepository, calendarRepository, appointmentRepository);
    }

    [Fact]
    public async Task Update_ShouldUpdated_WhenValidDataIsPassed()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(service);
        SetupNameExistsServiceRepository(false);

        // Act
        var result = await GetUpdatedServiceAsync(service);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenServiceIdDoesNotExist()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(null);

        // Act
        var result = await GetUpdatedServiceAsync(service);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ServiceNotFound.FirstError());
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenNameAlreadyExists()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(service);
        SetupNameExistsServiceRepository(true);

        // Act
        var result = await GetUpdatedServiceAsync(service);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.NameAlreadyExists.FirstError());
    }

    private async Task<Result<Service>> GetUpdatedServiceAsync(Service service)
    {
        var result = await _sut.UpdateServiceAsync(
            serviceId: service.Id,
            duration: service.Duration,
            name: service.Name,
            description: service.Description,
            colorScheme: service.ColorScheme);

        return result;
    }

    private void SetupServiceIdExistsServiceRepository(Service? returnValue)
    {
        _serviceRepository.GetByIdAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupNameExistsServiceRepository(bool returnValue)
    {
        _serviceRepository.NameExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
