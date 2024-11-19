using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Appointments.Policies;

public class AppointmentOverlapPolicyTests
{
    private readonly IAppointmentRepository _appointmentsRepository;
    private readonly AppointmentOverlapPolicy _sut;

    public AppointmentOverlapPolicyTests()
    {
        _appointmentsRepository = Substitute.For<IAppointmentRepository>();
        _sut = new AppointmentOverlapPolicy(_appointmentsRepository);
    }

    [Fact]
    public async Task IsOverlapping_ShouldReturnTrue_WhenOverlappingStrategyIsAllow()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.OverlappingStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.OverlappingOptions.AllowOverlapping);

        List<CalendarConfiguration> configurations = [configuration];

        _appointmentsRepository.IsOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsOverlappingAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            configurations,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsOverlapping_ShouldFailure_WhenOverlappingStrategyKeyNotFound()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.CreationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.CreationOptions.RequireConfirmation);

        List<CalendarConfiguration> configurations = [configuration];

        _appointmentsRepository.IsOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsOverlappingAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            configurations,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(CalendarConfigurationErrors.KeyNotFound.FirstError());
    }

    [Fact]
    public async Task IsOverlapping_ShouldReturnTrue_WhenOverlappingStrategyRejectIfOverlapping()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.OverlappingStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.OverlappingOptions.RejectIfOverlapping);

        List<CalendarConfiguration> configurations = [configuration];

        _appointmentsRepository.IsOverlappingAppointmentsAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<Period>(),
                Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _sut.IsOverlappingAsync(
            CalendarId.Create(),
            PeriodFactory.Create(),
            configurations,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
