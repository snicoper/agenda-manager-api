using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.Policies;
using AgendaManager.Domain.Calendars.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Appointments.Policies;

public class CalendarHasAppointmentsPolicyTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly HasAppointmentsInCalendarPolicy _sut;

    public CalendarHasAppointmentsPolicyTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _sut = new HasAppointmentsInCalendarPolicy(_appointmentRepository);
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnTrue_WhenAppointmentsExist()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _appointmentRepository.HasAppointmentsInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSatisfiedByAsync_ShouldReturnFalse_WhenNoAppointmentsExist()
    {
        // Arrange
        var calendarId = CalendarId.Create();
        _appointmentRepository.HasAppointmentsInCalendarAsync(calendarId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        // Act
        var result = await _sut.IsSatisfiedByAsync(calendarId, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
