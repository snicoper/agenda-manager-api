using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Calendars.Services.CalendarManager;

public class CalendarManagerDeleteTests : CalendarManagerBase
{
    [Fact]
    public async Task Delete_ShouldReturnSucceed_WhenValidParametersProvided()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupHasAppointmentsInCalendarPolicyIsSatisfiedBy(false);
        SetupHasResourcesInCalendarPolicyIsSatisfiedBy(false);
        SetupHasServicesInCalendarPolicyIsSatisfiedBy(false);

        // Act
        var result = await Sut.DeleteCalendarAsync(calendar, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailure_WhenAppointmentsExist()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupHasAppointmentsInCalendarPolicyIsSatisfiedBy(true);

        // Act
        var result = await Sut.DeleteCalendarAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailure_WhenResourcesExist()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupHasAppointmentsInCalendarPolicyIsSatisfiedBy(false);
        SetupHasResourcesInCalendarPolicyIsSatisfiedBy(true);

        // Act
        var result = await Sut.DeleteCalendarAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailure_WhenServicesExist()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        SetupHasAppointmentsInCalendarPolicyIsSatisfiedBy(false);
        SetupHasResourcesInCalendarPolicyIsSatisfiedBy(false);
        SetupHasServicesInCalendarPolicyIsSatisfiedBy(true);

        // Act
        var result = await Sut.DeleteCalendarAsync(calendar, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupHasAppointmentsInCalendarPolicyIsSatisfiedBy(bool exists)
    {
        AppointmentsInCalendarPolicy.IsSatisfiedByAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(exists);
    }

    private void SetupHasResourcesInCalendarPolicyIsSatisfiedBy(bool exists)
    {
        ResourcesInCalendarPolicy.IsSatisfiedByAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(exists);
    }

    private void SetupHasServicesInCalendarPolicyIsSatisfiedBy(bool exists)
    {
        ServicesInCalendarPolicy.IsSatisfiedByAsync(Arg.Any<CalendarId>(), Arg.Any<CancellationToken>())
            .Returns(exists);
    }
}
