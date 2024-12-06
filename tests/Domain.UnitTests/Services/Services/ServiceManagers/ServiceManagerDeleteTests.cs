using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Services;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Services.Services.ServiceManagers;

public class ServiceManagerDeleteTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ServiceManager _sut;

    public ServiceManagerDeleteTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        var calendarRepository = Substitute.For<ICalendarRepository>();

        _sut = new ServiceManager(_serviceRepository, calendarRepository, _appointmentRepository);
    }

    [Fact]
    public async Task ServiceManager_DeleteSucceeds_WhenServiceIsDeleted()
    {
        // Arrange
        var appointments = new List<Appointment>();
        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(service);
        SetupGetAllByServiceIdAppointmentRepository(appointments);

        // Act
        var result = await _sut.DeleteServiceAsync(service.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ServiceManager_DeleteFails_WhenServiceNotExists()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(null);

        // Act
        var result = await _sut.DeleteServiceAsync(service.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ServiceNotFound.FirstError());
    }

    [Fact]
    public async Task ServiceManager_DeleteFails_WhenHaveAppointmentsAssociated()
    {
        // Arrange
        var appointment = AppointmentFactory.CreateAppointment(
            period: Period.From(
                DateTimeOffset.Now,
                DateTimeOffset.Now.AddHours(1)));

        var service = ServiceFactory.CreateService();
        SetupServiceIdExistsServiceRepository(service);
        SetupGetAllByServiceIdAppointmentRepository([appointment.Value!]);

        // Act
        var result = await _sut.DeleteServiceAsync(service.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.HasAssociatedAppointments.FirstError());
    }

    private void SetupServiceIdExistsServiceRepository(Service? returnValue)
    {
        _serviceRepository.GetByIdAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupGetAllByServiceIdAppointmentRepository(List<Appointment> returnValue)
    {
        _appointmentRepository.GetAllByServiceId(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }
}
