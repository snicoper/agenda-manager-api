using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarConfigurations;

public class CalendarConfigurationUpdateTests
{
    [Fact]
    public void Update_ShouldReturnTrue_WhenValidParametersProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Act
        var updatedConfiguration = configuration.Update(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.AutoAccept);

        // Assert
        updatedConfiguration.Should().BeTrue();
    }

    [Fact]
    public void Update_ShouldReturnFalse_WhenSameConfigurationProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Act
        var updatedConfiguration = configuration.Update(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Assert
        updatedConfiguration.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Update_ShouldRaiseException_WhenInvalidCategoryProvided(int categoryLength)
    {
        // Arrange
        var category = new string('*', categoryLength);
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Act
        var action = () => configuration.Update(
            category: category,
            selectedKey: configuration.SelectedKey);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("The category must be between 0 and 100 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Update_ShouldRaiseException_WhenInvalidSelectedKeyProvided(int selectedKeyLength)
    {
        // Arrange
        var selectedKey = new string('*', selectedKeyLength);
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Act
        var action = () => configuration.Update(
            category: configuration.Category,
            selectedKey: selectedKey);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("The selected key must be between 0 and 100 characters.");
    }

    [Fact]
    public void Update_ShouldRaiseException_WhenInvalidConfigurationProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Act
        var action = () => configuration.Update(
            category: "CategoryNotExist",
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("Invalid configuration combination: CategoryNotExist - RequireConfirmation");
    }
}
